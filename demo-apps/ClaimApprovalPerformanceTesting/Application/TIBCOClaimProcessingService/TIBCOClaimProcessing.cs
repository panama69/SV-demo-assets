using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class ClaimProcessing : IClaimProcessing {

        private TIBCOMemberAccountsClient memberAccountsClient;
        private ApprovalClient approvalClient;
        private ExchangeRateClient exchangeRateClient;

        private readonly ClaimRegistry claimRegistry = ClaimRegistry.getInstance();

        public ClaimProcessing() {
            memberAccountsClient = new TIBCOMemberAccountsClient();

            approvalClient = new ApprovalClient();

            // update the URL of ApprovalService according to commandline argument
            String url = Config.getInstance().approvalServiceURL;
            if (url != null)
            {
                approvalClient.Endpoint.Address = new EndpointAddress(url);
            }

            exchangeRateClient = new ExchangeRateClient();

            // update the URL of ExchangeRateService according to commandline argument
            url = Config.getInstance().exchangeRateServiceURL;
            if (url != null)
            {
                exchangeRateClient.Endpoint.Address = new EndpointAddress(url);
            }

        }

        ~ClaimProcessing() {
            // approvalClient.Close();
            // exchangeRateClient.Close();
        }

        public ClaimId enterClaim(Claim claim) {
            Console.WriteLine("enterClaim(" + claim.firstName + ", " + claim.lastName + ", " + claim.socialSecurityNumber + ", $" + claim.claimedAmount + ")");

            IXMLMemberAccountsResponse response = null;
            try {
                response = memberAccountsClient.call(new IXMLMemberAccountsRequest()
                {
                    name = new ShortName { firstName = claim.firstName, lastName = claim.lastName },
                    dateOfBirth = new DateTime(0),
                    socialSecurityNumber = claim.socialSecurityNumber
                });
                Console.WriteLine("  -> found " + (response == null ? "0" : response.member.Length + " member" + (response == null || response.member.Length != 1 ? "s" : "")));
            } catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault("Error calling MemberAccountsService:\n" + e.Message));
            }

            Member[] found = response == null ? null : response.member;
            if (found == null || found.Length == 0) {
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault());
            } else if (found.Length > 1) {
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault("" + found.Length + " members matched, be more specific to select one."));
            }

            Detail detail = detail = found[0].detail;
            Console.WriteLine("  -> got " + detail.person.name + " details (" + detail.person.socialSecurityNumber + ", " + detail.person.dateOfBirth.ToShortDateString() + "...)");

            // for foreign accounts, get exchange rate
            float rate = 1.0f;
            if (!"USD".Equals(detail.currency))
            {
                rate = exchangeRateClient.getCurrentRate(new CurrencyPair() {from = detail.currency, to = Currency.USD});
                Console.WriteLine("  -> 1 " + detail.currency + " = " + rate + " USD");
            }

            // get user plan
            Plan plan = found[0].plan;
            float usdLimit = plan.approvalLimit*rate;
            Console.WriteLine("  -> got " + plan.name + " plan with $" + usdLimit + "(" + plan.approvalLimit + " " + detail.currency + ") approval limit");

            // fill in the user detail in the claim
            MemberId memberId = new MemberId(found[0].memberId);
            claim.firstName = detail.person.name.firstName;
            claim.lastName = detail.person.name.lastName;
            claim.socialSecurityNumber = detail.person.socialSecurityNumber;
            claim.memberId = memberId;

            // store the claim
            ClaimId claimId = claimRegistry.addClaim(claim, memberId);

            // is claim in limit of user plan
            bool inLimit = claim.claimedAmount <= usdLimit;

            // sanity check on plan limit
            if (plan.approvalLimit > 999999)
            {
                Console.WriteLine("  -> plan limit sanity check failed: limit can not be so high; the issue was logged");
                claim.approved = false;
                claim.approvalStatus = "rejected: internal error (illegal plan limit)";
            
            // check if user is in approval limit            
            } else if (inLimit)
            {
                claim.approved = true;
                claim.approvalStatus = "approved within plan limit";
            }
            else
            {

                try
                {
                    // if user is not in limit, approval service is called to approve procedure
                    ApprovalResult result;
                    approvalClient.approveProcedure(
                        claim.memberId.id,
                        claim.claimedAmount,
                        claimId.id,
                        new Customer() {firstName = claim.firstName, lastName = claim.lastName,},
                        null, // no medical history in TIBCO demo alternative
                        claim.description,
                        out result);
                    //Console.Out.WriteLine("result=" + result);
                    //Console.Out.WriteLine("result.equals(denied)=" + ApprovalResult.denied.Equals(result));
                    //Console.Out.WriteLine("result.equals(approved)=" + ApprovalResult.approved.Equals(result));
                    //Console.Out.WriteLine("result == denied =" + (ApprovalResult.denied == result));
                    //Console.Out.WriteLine("result == approved =" + (ApprovalResult.approved == result));
                    bool approved = result != ApprovalResult.denied;
                    claim.approved = approved;
                    claim.approvalStatus = "procedure " + (approved ? "approved" : "rejected") + " by approval service";

                    // if procedure was approved, approval service is called to approve payment
                    lock (approvalClient)
                    {
                        approvalClient.approvePayment(
                            claim.memberId.id,
                            claim.claimedAmount,
                            claimId.id,
                            new Customer() {firstName = claim.firstName, lastName = claim.lastName},
                            out result);
                    }
                    //Console.Out.WriteLine("result=" + result);
                    //Console.Out.WriteLine("result.equals(denied)=" + ApprovalResult.denied.Equals(result));
                    //Console.Out.WriteLine("result.equals(approved)=" + ApprovalResult.approved.Equals(result));
                    //Console.Out.WriteLine("result == denied =" + (ApprovalResult.denied == result));
                    //Console.Out.WriteLine("result == approved =" + (ApprovalResult.approved == result));
                    approved = result != ApprovalResult.denied;
                    claim.approved = approved;
                    claim.approvalStatus = "; payment " + (approved ? "approved" : "rejected") + " by approval service";                        


                }
                catch (Exception e)
                {
                    Console.WriteLine("Error calling ApprovalService:\n" + e);
                    claim.approvalStatus = "rejected: Error calling ApprovalService";
                }
            }

            Console.WriteLine("  -> " + claimId + ": " + claim);
            return claimId;
        }


        public ClaimId[] listClaims(MemberId memberId) {
            Console.WriteLine("listClaims(" + memberId + ")");
            ISet<ClaimId> claimIds;
            if (claimRegistry.listClaims(memberId, out claimIds)) {
                return claimIds.ToArray();
            } else {
                return new ClaimId[0];
            }
        }


        public Claim getClaim(ClaimId claimId) {
            Console.WriteLine("getClaim(" + claimId + ")");
            Claim claim = null;
            if (claimRegistry.getClaim(claimId, out claim))
            {
                return claim;
            } else {
                throw new FaultException<ClaimNotFoundFault>(new ClaimNotFoundFault("Claim " + claimId + " not found."));
            }
        }

    }

}
