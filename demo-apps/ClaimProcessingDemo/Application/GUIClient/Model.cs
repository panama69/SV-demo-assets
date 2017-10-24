using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {
    public class Model {

        private Claim[] claims;
        private int currentClaim = 0;

        public Model() {
            generateDefaultData();
        }


        public bool hasNextClaim() {
            return currentClaim < claims.Length - 1;
        }

        public bool hasPrevClaim() {
            return currentClaim > 0;
        }

        public Claim getCurrentClaim() {
            return claims[currentClaim];
        }

        public void next() {
            if (hasNextClaim()) {
                currentClaim++;
            }
        }

        public void prev() {
            if (hasPrevClaim()) {
                currentClaim--;
            }
        }

        #region default data generation
        private void generateDefaultData() {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim {
                firstName = "Hercule",
                lastName = "Poirot",
                socialSecurityNumber = "554-98-0001",
                date = new DateTime(2010, 1, 1),
                description = "premolar seal",
                claimedAmount = 24
            });
            claims.Add(new Claim {
                firstName = "Hercule",
                lastName = "Poirot",
                date = new DateTime(2010, 1, 15),
                description = "premolar extraction",
                claimedAmount = 2000
            });

            claims.Add(new Claim {
                lastName = "Poirot",
                date = new DateTime(2010, 1, 15),
                description = "(fail: not unique)",
                claimedAmount = 10
            });
            claims.Add(new Claim {
                firstName = "Karel",
                lastName = "Got (fail: not in system)",
                date = new DateTime(2010, 1, 1),
                description = "Klacid 500mg tbl 14",
                claimedAmount = 29.90F
            });

            claims.Add(new Claim {
                firstName = "Sherlock",
                lastName = "Holmes",
                socialSecurityNumber = "332-10-0002",
                date = new DateTime(2010, 2, 1),
                description = "ear surgery",
                claimedAmount = 400
            });
            claims.Add(new Claim {
                socialSecurityNumber = "332-10-0002",
                date = new DateTime(2010, 2, 1),
                description = "eye surgery",
                claimedAmount = 800
            });
            claims.Add(new Claim {
                socialSecurityNumber = "332-10-0002",
                date = new DateTime(2010, 2, 1),
                description = "nose surgery",
                claimedAmount = 900
            });

            claims.Add(new Claim {
                firstName = "Albert",
                lastName = "Einstein",
                socialSecurityNumber = "809-42-0002",
                date = new DateTime(2010, 3, 1),
                description = "Donormyl 25mg tbl. 96",
                claimedAmount = 69.90F
            });
            claims.Add(new Claim {
                socialSecurityNumber = "809-42-0002",
                date = new DateTime(2010, 3, 8),
                description = "2x Donormyl 25mg tbl. 96",
                claimedAmount = 139.80F
            });
            claims.Add(new Claim {
                lastName = "Einstein",
                date = new DateTime(2010, 3, 15),
                description = "Prozac 20mg tbl. 30",
                claimedAmount = 49.90F
            });

            this.claims = claims.ToArray<Claim>();
        }
        #endregion

    }
}
