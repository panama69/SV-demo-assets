using System;

using System.ServiceModel;
using System.Runtime.Serialization;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceContract(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", SessionMode = SessionMode.NotAllowed)]
    public interface IMemberAccounts {

        [OperationContract]
        Member[] memberSearch(ShortName name, DateTime dateOfBirth, string socialSecurityNumber, string zipCode);

        [OperationContract]
        Plan getMemberPlan(long memberId);

        [OperationContract]
        Detail getMemberDetail(long memberId);

        [OperationContract]
        MedicalHistory getMemberMedicalHistory(long memberId);

    }


    [DataContract]
    public class Member {
        [DataMember]
        public long memberId { get; set; }
        [DataMember]
        public long householdId { get; set; }
        [DataMember]
        public String socialSecurityNumber { get; set; }

        public override string ToString() {
            return "Member[memberId=" + memberId + ", householdId=" + householdId + ", ssn=" + socialSecurityNumber + "]";
        }
    }


    [DataContract]
    public class Plan {
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public float approvalLimit { get; set; }

        public override string ToString() {
            return "Plan[in=" + id + ", name=" + name + ", approvalLimit=" + approvalLimit + "]";
        }
    }

    [DataContract]
    public class Detail {
        [DataMember]
        public long householdId { get; set; }
        [DataMember]
        public long memberId { get; set; }
        [DataMember]
        public Person person { get; set; }
        [DataMember]
        public Address address { get; set; }
        [DataMember]
        public Phone phone { get; set; }

        [DataMember]
        public Currency currency { get; set; }

        public override string ToString() {
            return "Detail[householdId=" + householdId + ", memberId=" + memberId + ", currency=" + currency + ", person=" + person + "]";
        }
    }


    [DataContract]
    public class Person {
        [DataMember]
        public Name name { get; set; }
        [DataMember]
        public DateTime dateOfBirth { get; set; }
        [DataMember]
        public string socialSecurityNumber { get; set; }
        [DataMember]
        public Gender gender { get; set; }

        public override string ToString() {
            return "Person[name=" + name + ", dateOfBirth=" + dateOfBirth.ToShortDateString() + "]";
        }
    }


    [DataContract]
    public class ShortName {
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }

        public override string ToString() {
            return "ShortName[" + firstName + " " + lastName + "]";
        }
    }


    [DataContract]
    public class Name {
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string suffix { get; set; }

        public override string ToString()         {
            return "Name[" + notNull(prefix) + firstName + " " + notNull(middleName) + lastName + notNull(suffix) + "]";
        }

        private static String notNull(String s) {
            return (s == null ? "" : (s + " "));
        }
    }


    public enum Gender {
        MALE,
        FEMALE,
        UNKNOWN
    }


    [DataContract]
    public class Address {
        [DataMember]
        public DateTime startDate { get; set; }
        [DataMember]
        public DateTime endDate { get; set; }
        [DataMember]
        public string line1  { get; set; }
        [DataMember]
        public string line2  { get; set; }
        [DataMember]
        public string city  { get; set; }
        [DataMember]
        public string state  { get; set; }
        [DataMember]
        public string country  { get; set; }
        [DataMember]
        public string zip  { get; set; }
    }


    [DataContract]
    public class Phone {
        [DataMember]
        public PhoneNumberType phoneNumberType { get; set; }
        [DataMember]
        public string phoneNumber { get; set; }
        [DataMember]
        public string extension { get; set; }
    }

    [DataContract]
    public enum PhoneNumberType
    {
        [EnumMember]
        DAY,
        [EnumMember]
        EVENING
    }

}
