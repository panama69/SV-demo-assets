using System;
using System.IO;

using System.Runtime.Serialization;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [DataContract]
    public class MedicalHistory {
        [DataMember]
        public long memberId { get; set; }

        [DataMember]
        public Treatment[] treatments { get; set; }

        [DataMember]
        public Examination[] reviewOfSystems { get; set; }

        [DataMember]
        public Disease[] familyDiseases { get; set; }

        [DataMember]
        public Disease[] childhoodDiseases { get; set; }

        [DataMember]
        public Medication[] medications { get; set; }

    }


    [DataContract]
    public class Treatment{
        [DataMember]
        public Disease disease { get; set; }

        [DataMember]
        public Medication[] medications { get; set; }

        [DataMember]
        public Examination[] examinations { get; set; }
    }

    [DataContract]
    public class Examination
    {
        [DataMember]
        public long examCode { get; set; }

        [DataMember]
        public String examName { get; set; }

        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        public String result { get; set; }

        [DataMember]
        public ScannedDocument[] documents { get; set; }
    }


    [DataContract]
    public class Disease {
        [DataMember]
        public long diagnosisCode { get; set; }
        [DataMember]
        public String diagnosis { get; set; }

        [DataMember]
        public DatedRecord chiefComplaint { get; set; }

        [DataMember]
        public DatedRecord[] historyOfPresentIllness { get; set; }
    }

    [DataContract]
    public class DatedRecord {
        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        public String record { get; set; }
    }


    [DataContract]
    public class Medication {
        [DataMember]
        public long prescriptionCode { get; set; }

        [DataMember]
        public String name { get; set; }
    }


    [DataContract]
    public class ScannedDocument {
        [DataMember]
        public String documentType { get; set; }

        [DataMember]
        public DateTime date { get; set; }

        [DataMember]
        public byte[] data { get; set; }

    }



}
