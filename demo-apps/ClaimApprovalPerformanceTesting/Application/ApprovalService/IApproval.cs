using System.ServiceModel;
using System.Runtime.Serialization;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceContract(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", SessionMode = SessionMode.NotAllowed)]
    public interface IApproval {
        [OperationContract]
        ApprovalResponse approveProcedure(ProcedureRequest request);

        [OperationContract]
        ApprovalResponse approvePayment(PaymentRequest request);
    }

    [MessageContract]
    public class ProcedureRequest:PaymentRequest
    {
        [MessageBodyMember]
        public string procedure { get; set; }
        [MessageBodyMember]
        public MedicalHistory medicalHistory { get; set; }
    }

    [MessageContract]
    public class PaymentRequest
    {
        [MessageHeader]
        public long memberId { get; set; }

        [MessageBodyMember]
        public Customer customer { get; set; }
        [MessageBodyMember]
        public long claimId { get; set; }
        [MessageBodyMember]
        public float amount { get; set; }
    }

    [DataContract]
    public class Customer {
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }

        public override string ToString() {
            return "Customer[" + firstName + " " + lastName + "]";
        }
    }

    [MessageContract]
    public class ApprovalResponse {
        [MessageBodyMember]
        public ApprovalResult result { get; set; }

        [MessageBodyMember]
        public string description { get; set; }

        public override string ToString() {
            return "ApprovalResponse[" + result + ": " + description  + "]";
        }
    }

    [DataContract]
    public enum ProcedureType {
        [EnumMember]
        preventive,
        [EnumMember]
        addiction,
        [EnumMember]
        transplant,
        [EnumMember]
        other
    }


    [DataContract]
    public enum ApprovalResult {
        [EnumMember]
        denied,

        [EnumMember]
        approved
    }


}
