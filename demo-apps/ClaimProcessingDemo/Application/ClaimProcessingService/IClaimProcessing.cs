using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceContract(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", SessionMode = SessionMode.NotAllowed)]
    public interface IClaimProcessing {

        [OperationContract]
        [FaultContract(typeof(MemberNotFoundFault), Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01")]
        ClaimId enterClaim(Claim claim);

        [OperationContract]
        ClaimId[] listClaims(MemberId memberId);

        [OperationContract]
        [FaultContract(typeof(ClaimNotFoundFault), Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01")]
        Claim getClaim(ClaimId claimId);
    }


    [DataContract]
    public class ClaimId {

        [DataMember]
        public long id { get; set; }

        public ClaimId(long id) {
            this.id = id;
        }

        public override int GetHashCode() {
            return (int)(id & 0xFFFFFFFF);
        }

        public override bool Equals(object obj) {
            return id == ((ClaimId)obj).id;
        }

        public override string ToString() {
            return "#" + id;
        }
    }


    [DataContract]
    public class MemberId {

        [DataMember]
        public long id { get; set; }

        public MemberId(long id) {
            this.id = id;
        }

        public override int GetHashCode() {
            return (int) (id & 0xFFFFFFFF);
        }

        public override bool Equals(object obj) {
            return id == ((MemberId)obj).id;
        }

        public override string ToString() {
            return "#" + id;
        }
    }


    [DataContract]
    public class Claim {

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string lastName { get; set; }

        [DataMember]
        public string socialSecurityNumber { get; set; }

        [DataMember]
        public MemberId memberId { get; set; }

        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public float claimedAmount { get; set; }

        [DataMember]
        public float adjustedAmount { get; set; }

        [DataMember]
        public bool approved { get; set; }

        [DataMember]
        public string approvalStatus { get; set; }

        [DataMember]
        public bool reimbursed { get; set; }

        public override string ToString() {
            return "Claim(" + firstName + " " + lastName + ", " + socialSecurityNumber + ", " + date.ToShortDateString() + ", $" + claimedAmount + ": " + (approved ? "" : "not ") + "approved), " + approvalStatus;
        }
    }


    [DataContract]
    public class MemberNotFoundFault {

        [DataMember]
        public string Reason { get; set; }

        public MemberNotFoundFault() : this("Member not found.") {
        }

        public MemberNotFoundFault(string reason) {
            this.Reason = reason;
        }
    }


    [DataContract]
    public class ClaimNotFoundFault {

        [DataMember]
        public string Reason { get; set; }

        public ClaimNotFoundFault() : this("Claim not found.") {
        }

        public ClaimNotFoundFault(string reason) {
            this.Reason = reason;
        }
    }

}
