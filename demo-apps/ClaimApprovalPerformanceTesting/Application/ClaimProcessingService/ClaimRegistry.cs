using System.Collections.Generic;

namespace HP.SOAQ.ServiceSimulation.Demo {
    class ClaimRegistry{

        private static readonly ClaimRegistry INSTANCE = new ClaimRegistry();

        private readonly IDictionary<ClaimId, Claim> claimsById = new Dictionary<ClaimId, Claim>();
        private readonly IDictionary<MemberId, ISet<ClaimId>> membersClaimIds = new Dictionary<MemberId, ISet<ClaimId>>();

        private long nextClaimId = 0;


        private ClaimRegistry()
        {
        }

        
        public static ClaimRegistry getInstance() {
            return INSTANCE;
        }


        public ClaimId addClaim(Claim claim, MemberId memberId) {           
            lock(claimsById) {
                ClaimId claimId = new ClaimId(nextClaimId++);

                // store the claim
                claimsById.Add(claimId, claim);

                // update list of user's claims
                ISet<ClaimId> claimIds;
                if (membersClaimIds.TryGetValue(memberId, out claimIds))
                {
                    claimIds.Add(claimId);
                }
                else
                {
                    claimIds = new HashSet<ClaimId>();
                    claimIds.Add(claimId);
                    membersClaimIds.Add(memberId, claimIds);
                }
                return claimId;
            }
        }


        public bool listClaims(MemberId memberId, out ISet<ClaimId> claimIds) {
            lock (claimsById) {
                return membersClaimIds.TryGetValue(memberId, out claimIds);
            }
        }


        public bool getClaim(ClaimId claimId, out Claim claim) {
            lock (claimsById) {
                return claimsById.TryGetValue(claimId, out claim);
            }
        }

    }
}
