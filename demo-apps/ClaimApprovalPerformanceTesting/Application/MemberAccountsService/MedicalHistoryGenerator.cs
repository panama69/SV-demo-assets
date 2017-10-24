using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {
    internal class MedicalHistoryGenerator
    {
        public static readonly MedicalHistoryGenerator INSTANCE = new MedicalHistoryGenerator();

        private readonly Random RNG = new Random(0);

        private MedicalHistoryGenerator()
        {
        }

        public MedicalHistory createMedicalHistory(long memberID)
        {
            MedicalHistory result =
                new MedicalHistory()
                    {
                        memberId = memberID,
                        familyDiseases = createDiseases(),
                        childhoodDiseases = createDiseases(),
                        medications = createMedications(),
                        reviewOfSystems = createExams(),
                        treatments = createTreatments()
                    };

            return result;
        }

        private Disease[] createDiseases()
        {
            if (RNG.Next(3) == 0) return null;  // healthy
            var result = new Disease[1 + RNG.Next(4)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createDisease();
            return result;
        }

        private Medication[] createMedications()
        {
            if (RNG.Next(3) == 0) return null;  // no meds
            var result = new Medication[1 + RNG.Next(10)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createMedication();
            return result;
        }

        private Examination[] createExams()
        {
            if (RNG.Next(3) == 0) return null;  // no examination
            var result = new Examination[1 + RNG.Next(4)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createExamination();
            return result;
        }

        private Treatment[] createTreatments()
        {
            if (RNG.Next(3) == 0) return null;  // no treatment
            var result = new Treatment[1 + RNG.Next(10)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createTreatment();
            return result;
        }

        private DatedRecord[] createDatedRecords()
        {
            if (RNG.Next(3) == 0) return null;  // no treatment
            var result = new DatedRecord[1 + RNG.Next(10)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createDatedRecord();
            return result;
        }


        private Disease createDisease()
        {
            return new Disease()
            {
                diagnosis = createSentences(1),
                diagnosisCode = RNG.Next(1000),
                historyOfPresentIllness = createDatedRecords(),
                chiefComplaint = createDatedRecord()
            };
        }

        private Medication createMedication()
        {
            return new Medication()
            {
                name = createSentences(1),
                prescriptionCode = RNG.Next(1000)
            };
        }

        private Examination createExamination()
        {
            return new Examination()
            {
                date = createDate(),
                documents = createDocuments(),
                examCode = RNG.Next(1000),
                examName = createSentences(1),
                result = createSentences(1 + RNG.Next(5))
            };
        }

        private Treatment createTreatment()
        {
            return new Treatment()
            {
                disease = createDisease(),
                examinations = createExams(),
                medications = createMedications()
            };
        }

        private DatedRecord createDatedRecord()
        {
            return new DatedRecord()
            {
                date = createDate(),
                record = createSentences(1 + RNG.Next(5))
            };
        }

        private DateTime createDate()
        {
            return new DateTime(1880 + RNG.Next(50), 1 + RNG.Next(12), 1 + RNG.Next(28));
        }

        private ScannedDocument[] createDocuments()
        {
            if (RNG.Next(3) < 2) return null;  // no documents
            var result = new ScannedDocument[1 + RNG.Next(3)];
            for (int i = 0; i < result.Length; i++)
                result[i] = createDocument();
            return result;
        }

        private ScannedDocument createDocument()
        {
            return new ScannedDocument()
                       {
                           date = createDate(),
                           data = createBytes(),
                           documentType = "jpeg"
                       };
        }

        private byte[] createBytes()
        {
            byte[] result = new byte[RNG.Next(100)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ((byte) ('A' + RNG.Next(27)));
            }
            return result;
        }


        private readonly string[] SENTENCES =
            {
                "It seemed to me that I had never met so fascinating and so thoughtful a man." ,
                "As I was already in debt to my tradesmen, the advance was a great convenience, and yet there was something unnatural about the whole transaction which made me wish to know a little more before I quite committed myself." ,
                "It is the most lovely country, my dear young lady, and the dearest old country-house.",
                "I should be glad to know what they would be.",
                "I was a little startled at the nature of the child's amusement, but the father's laughter made me think that perhaps he was joking."
            };

        public string createSentences(int count)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(SENTENCES[RNG.Next(SENTENCES.Length)]);
                sb.Append(' ');
            }
            return sb.ToString();
        }

    }
}
