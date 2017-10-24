using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.ServiceModel;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ClaimProcessing : IClaimProcessing {

        private MemberAccountsClient memberAccountsClient;

        private readonly IDictionary<ClaimId, Claim> claimsById = new Dictionary<ClaimId, Claim>();
        private readonly IDictionary<MemberId, ISet<ClaimId>> membersClaimIds = new Dictionary<MemberId, ISet<ClaimId>>();

        private long nextClaimId = 0;


        public ClaimProcessing() {
            if (Config.GetInstance().Type.Equals(HttpClientCredentialType.None)) {
                CreateNone();
            } else if (Config.GetInstance().Type.Equals(HttpClientCredentialType.Basic)) {
                CreateBasic();
            } else if (Config.GetInstance().Type.Equals(HttpClientCredentialType.Ntlm)) {
                CreateNtlm();
            }
        }

        private void CreateNone() {
            memberAccountsClient = new MemberAccountsClient();

            // update the URL of MemberAccountsService according to commandline argument
            String url = Config.GetInstance().ServiceUrl;
            if (url != null) {
                memberAccountsClient.Endpoint.Address = new EndpointAddress(url);
            }
        }

        private void CreateBasic() {
            CreateSecurityBase(HttpClientCredentialType.Basic);
            // credentials for Basic authentication
            memberAccountsClient.ClientCredentials.UserName.UserName = "hpguest"; // local user
            memberAccountsClient.ClientCredentials.UserName.Password = "hpguest";
        }

        private void CreateNtlm() {
            CreateSecurityBase(HttpClientCredentialType.Ntlm);

            // credentials for NTLM authentication
            memberAccountsClient.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.None;
            memberAccountsClient.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("hpguest", "hpguest", "");
        }

        private void CreateSecurityBase(HttpClientCredentialType type) {
            BasicHttpBinding binding = new BasicHttpBinding() { Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01" };
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = type;

            String sUrl = Config.GetInstance().ServiceUrl;
            EndpointAddress url;
            if (sUrl != null) {
                url = new EndpointAddress(sUrl);
            } else {
                memberAccountsClient = new MemberAccountsClient();

                url = memberAccountsClient.Endpoint.Address;
            }

            memberAccountsClient = new MemberAccountsClient(binding, url);
        }

        ~ClaimProcessing() {
            memberAccountsClient.Close();
        }


        public ClaimId enterClaim(Claim claim) {
            Console.WriteLine("enterClaim(" + claim.firstName + ", " + claim.lastName + ", " +
                              claim.socialSecurityNumber + ", $" + claim.claimedAmount + ")");

            Member[] found = null;
            try {
                DateTime callStart = DateTime.Now;
                found =
                    memberAccountsClient.memberSearch(
                        new ShortName() {firstName = claim.firstName, lastName = claim.lastName}, new DateTime(0),
                        claim.socialSecurityNumber, null);
                DateTime callEnd = DateTime.Now;

                Console.WriteLine(string.Format("  -> found {0} member{1}{2}.",
                                                (found == null ? "0" : found.Length.ToString()),
                                                (found == null || found.Length != 1 ? "s" : ""),
                                                GetResponseTimeString(callStart, callEnd)));
            } catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
                throw new FaultException<MemberNotFoundFault>(
                    new MemberNotFoundFault("Error calling MemberAccountsService:\n" + e.Message));
            }

            if (found == null || found.Length == 0) {
                throw new FaultException<MemberNotFoundFault>(new MemberNotFoundFault());
            } else if (found.Length > 1) {
                throw new FaultException<MemberNotFoundFault>(
                    new MemberNotFoundFault("" + found.Length + " members matched, be more specific to select one."));
            }

            // get user plan
            Plan plan = null;
            try {
                DateTime callStart = DateTime.Now;
                plan = memberAccountsClient.getMemberPlan(found[0].memberId);
                DateTime callEnd = DateTime.Now;

                Console.WriteLine(string.Format("  -> got {0} plan with ${1} approval limit{2}.", plan.name,
                                                plan.approvalLimit,
                                                GetResponseTimeString(callStart, callEnd)));
            } catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
            }

            // check the limit and approve claim if the claimed amount is not higher
            claim.approved = claim.claimedAmount <= plan.approvalLimit;
            claim.approvalStatus = claim.approved ? "approved" : "needs further approval";
            // get user details
            Detail detail = null;
            try {
                DateTime callStart = DateTime.Now;
                detail = memberAccountsClient.getMemberDetail(found[0].memberId);
                DateTime callEnd = DateTime.Now;

                Console.WriteLine("  -> got " + detail.person.name + " details (" + detail.person.socialSecurityNumber +
                                  ", " + detail.person.dateOfBirth.ToShortDateString() + "...)" +
                                  GetResponseTimeString(callStart, callEnd) + '.');
            } catch (Exception e) {
                Console.WriteLine("Error calling MemberAccountsService:\n" + e);
            }

            // fill in the user detail in the claim
            ClaimId claimId = new ClaimId(nextClaimId++);
            MemberId memberId = new MemberId(found[0].memberId);
            claim.firstName = detail.person.name.firstName;
            claim.lastName = detail.person.name.lastName;
            claim.socialSecurityNumber = detail.person.socialSecurityNumber;
            claim.memberId = memberId;

            // store the claim
            claimsById.Add(claimId, claim);

            // update list of user's claims
            ISet<ClaimId> claimIds;
            if (membersClaimIds.TryGetValue(memberId, out claimIds)) {
                claimIds.Add(claimId);
            } else {
                claimIds = new HashSet<ClaimId>();
                claimIds.Add(claimId);
                membersClaimIds.Add(memberId, claimIds);
            }

            Console.WriteLine("  -> " + claimId + ": " + claim);
            return claimId;
        }

        private static string GetResponseTimeString(DateTime callStart, DateTime callEnd) {
            return string.Format(" (response) time: {0} ms)", (long)((callEnd - callStart).TotalMilliseconds));
        }


        public ClaimId[] listClaims(MemberId memberId) {
            Console.WriteLine("listClaims(" + memberId + ")");
            ISet<ClaimId> claimIds;
            if (membersClaimIds.TryGetValue(memberId, out claimIds)) {
                return claimIds.ToArray();
            } else {
                return new ClaimId[0];
            }
        }

        public Claim getClaim(ClaimId claimId) {
            Console.WriteLine("getClaim(" + claimId + ")");
            Claim claim = null;
            if (claimsById.TryGetValue(claimId, out claim)) {
                return claim;
            } else {
                throw new FaultException<ClaimNotFoundFault>(new ClaimNotFoundFault("Claim " + claimId + " not found."));
            }
        }

    }

}
