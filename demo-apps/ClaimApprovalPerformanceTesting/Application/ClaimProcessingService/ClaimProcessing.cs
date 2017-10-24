using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ClaimProcessing : IClaimProcessing {

        private MemberAccountsClient memberAccountsClient;
        private ApprovalClient approvalClient;
        private ExchangeRateClient exchangeRateClient;

        private readonly ClaimRegistry claimRegistry = ClaimRegistry.getInstance();

        public ClaimProcessing() {
            memberAccountsClient = new MemberAccountsClient();

            // update the URL of MemberAccountsService according to commandline argument
            String url = Config.getInstance().memberAcoountsServiceURL;
            if (url != null) {
                memberAccountsClient.Endpoint.Address = new EndpointAddress(url);
            }

            approvalClient = new ApprovalClient();

            // update the URL of ApprovalService according to commandline argument
            url = Config.getInstance().approvalServiceURL;
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
            memberAccountsClient.Close();
        }


        public ClaimId enterClaim(Claim claim) {
            Console.WriteLine("enterClaim(" + claim.firstName + ", " + claim.lastName + ", " + claim.socialSecurityNumber + ", $" + claim.claimedAmount + ")");

            Member[] found = null;
            try {
                found = memberAccountsClient.memberSearch(new ShortName {firstName = claim.firstName, lastName = claim.lastName}, new DateTime(0), claim.socialSecurityNumber, null);
                Console.WriteLine("  -> found " + (found == null ? "0" : found.Length + " member" + (found == null || found.Length != 1 ? "s" : "")));
            } catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault("Error calling MemberAccountsService:\n" + e.Message));
            } 
            
            if (found == null || found.Length == 0) {
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault());
            } else if (found.Length > 1) {
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault("" + found.Length + " members matched, be more specific to select one."));
            }

            // get user details
            Detail detail = null;
            try
            {
                detail = memberAccountsClient.getMemberDetail(found[0].memberId);
                Console.WriteLine("  -> got " + detail.person.name + " details (" + detail.person.socialSecurityNumber + ", " + detail.person.dateOfBirth.ToShortDateString() + "...)");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
            }

            // for foreign accounts, get exchange rate
            float rate = 1.0f;
            if (!"USD".Equals(detail.currency))
            {
                rate = exchangeRateClient.getCurrentRate(new CurrencyPair() {from = detail.currency, to = Currency.USD});
                Console.WriteLine("  -> 1 " + detail.currency + " = " + rate + " USD");
            }

            // get user plan
            Plan plan = null;
            float usdLimit = 0;
            try {
                plan = memberAccountsClient.getMemberPlan(found[0].memberId);
                usdLimit = plan.approvalLimit*rate;
                Console.WriteLine("  -> got " + plan.name + " plan with $" + usdLimit + "(" + plan.approvalLimit + " " + detail.currency + ") approval limit");
            }
            catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
            }

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

            // check if user is in approval limit
            if (inLimit)
            {
                claim.approved = true;
                claim.approvalStatus = "approved within plan limit";
            }
            else
            {
                // if user is not in limit, get his medical history
                MedicalHistory medicalHistory = null;
                try
                {
                    medicalHistory = memberAccountsClient.getMemberMedicalHistory(found[0].memberId);
                    Console.WriteLine("  -> got medical history");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error calling MemberAccountsService:\n" + e);
                }

                try
                {
                    // if user is not in limit, approval service is called to approve procedure
                    ApprovalResult result;
                    approvalClient.approveProcedure(
                        claim.memberId.id,
                        claim.claimedAmount,
                        claimId.id,
                        new Customer() { firstName = claim.firstName, lastName = claim.lastName, },
                        medicalHistory, 
                        claim.description,                          
                        out result);
                    bool approved = result == ApprovalResult.approved;
                    claim.approved = approved;
                    claim.approvalStatus = "procedure " + (approved ? "approved" : "rejected") + " by approval service";

                    if (approved)
                    {
                        // if procedure was approved, approval service is called to approve payment
                        approvalClient.approvePayment(
                            claim.memberId.id, 
                            claim.claimedAmount,
                            claimId.id,
                            new Customer() { firstName = claim.firstName, lastName = claim.lastName },
                            out result);
                        approved = result == ApprovalResult.approved;
                        claim.approved = approved;
                        claim.approvalStatus = "; payment " + (approved ? "approved" : "rejected") + " by approval service";                        
                    }

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
