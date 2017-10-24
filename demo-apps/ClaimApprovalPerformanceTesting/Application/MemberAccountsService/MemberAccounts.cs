using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MemberAccounts : IMemberAccounts {

        #region performance settings
        private static readonly int MEMBER_SEARCH_RESPONSE_TIME = 200;  // + 100ms Shared resource processing time, see SharedDBConnection
        private static readonly int GET_MEMBER_PLAN_RESPONSE_TIME = 30;
        private static readonly int GET_MEMBER_DETAIL_RESPONSE_TIME = 100;
        private static readonly int GET_MEMBER_MED_HISTORY_RESPONSE_TIME = 600;
        #endregion

        private readonly IDictionary<string, ISet<Member>> membersByFirstName = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersByLastName = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersBySSN = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersByZipCode = new Dictionary<string, ISet<Member>>();

        private readonly IDictionary<long, Plan> plansByMemderId = new Dictionary<long, Plan>();
        private readonly IDictionary<long, Detail> detailsByMemderId = new Dictionary<long, Detail>();
        private readonly IDictionary<long, MedicalHistory> medicalHistoryByMemderId = new Dictionary<long, MedicalHistory>();

        public MemberAccounts() {
            generateDefaultData();
        }


        public virtual Member[] memberSearch(ShortName name, DateTime dateOfBirth, string socialSecurityNumber, string zipCode) {
            Console.Write("memberSearch(" + name.firstName + ", " + name.lastName + ", " + socialSecurityNumber + ") ");
            Thread.Sleep(MEMBER_SEARCH_RESPONSE_TIME);

            ISet<Member> members = null;

            if (socialSecurityNumber != null) {
                membersBySSN.TryGetValue(socialSecurityNumber, out members);
            }
            if (members == null && name.lastName != null) {
                membersByLastName.TryGetValue(name.lastName, out members);
            }
            if (members == null && name.firstName != null) {
                membersByFirstName.TryGetValue(name.firstName, out members);
            }
            if (members == null && zipCode != null) {
                membersByZipCode.TryGetValue(zipCode, out members);
            }

            if (members != null) {
                Member[] result = filterMembers(members.ToArray<Member>(), name, dateOfBirth, socialSecurityNumber, zipCode);
                Console.WriteLine("-> " + result.Length + " result" + ((result.Length == 1) ? "" : "s"));
                return result;
            } else {
                Console.WriteLine("-> not found");
                return new Member[0];
            }
        }


        public virtual Plan getMemberPlan(long memberId) {
            Plan plan = plansByMemderId[memberId];
            Console.WriteLine("-> member #" + memberId + " has " + plan);
            Thread.Sleep(GET_MEMBER_PLAN_RESPONSE_TIME);
            return plan;
        }


        public virtual Detail getMemberDetail(long memberId) {
            Detail detail = detailsByMemderId[memberId];
            Console.WriteLine("-> member #" + memberId + " is " + detail);
            Thread.Sleep(GET_MEMBER_DETAIL_RESPONSE_TIME);
            return detail;
        }

        public MedicalHistory getMemberMedicalHistory(long memberId)
        {
            Console.WriteLine("-> returning member #" + memberId + " medical history");
            Thread.Sleep(GET_MEMBER_MED_HISTORY_RESPONSE_TIME);
            return MedicalHistoryGenerator.INSTANCE.createMedicalHistory(memberId);//medicalHistoryByMemderId[memberId];
        }


        private static bool undef(string s) {
            return (s == null) || (s.Length == 0);
        }

        private static bool undef(DateTime d) {
            return (d == null) || (d.Equals(new DateTime(0)));
        }

        private Member[] filterMembers(Member[] src, ShortName name, DateTime dateOfBirth, string socialSecurityNumber, string zipCode) {
            List<Member> result = new List<Member>();

            for (int i = 0; i < src.Length; i++) {
                Member member = src[i];
                Detail detail = detailsByMemderId[member.memberId];

                if ((undef(name.firstName) || name.firstName.Equals(detail.person.name.firstName)) &&
                    (undef(name.lastName) || name.lastName.Equals(detail.person.name.lastName)) &&
                    (undef(dateOfBirth) || dateOfBirth.Equals(detail.person.dateOfBirth)) &&
                    (undef(socialSecurityNumber) || socialSecurityNumber.Equals(detail.person.socialSecurityNumber)) &&
                    (undef(zipCode) || zipCode.Equals(detail.address.zip))) {
                    result.Add(member);
                }
            }

            return result.ToArray<Member>();
        }


        private void addToMultiMap(IDictionary<string, ISet<Member>> map, string key, Member value) {
            if (key != null) {
                ISet<Member> values;
                if (map.TryGetValue(key, out values)) {
                    values.Add(value);
                } else {
                    values = new HashSet<Member>();
                    values.Add(value);
                    map.Add(key, values);
                }
            }
        }


        private void add(Detail d, Plan p) {
            Member m = new Member() {
                householdId = d.householdId,
                memberId = d.memberId,
                socialSecurityNumber = d.person.socialSecurityNumber
            };
             
            addToMultiMap(membersByFirstName, d.person.name.firstName, m);
            addToMultiMap(membersByLastName, d.person.name.lastName, m);
            addToMultiMap(membersBySSN, d.person.socialSecurityNumber, m);
            addToMultiMap(membersByZipCode, d.address.zip, m);

            plansByMemderId.Add(d.memberId, p);
            detailsByMemderId.Add(d.memberId, d);
            medicalHistoryByMemderId.Add(d.memberId, MedicalHistoryGenerator.INSTANCE.createMedicalHistory(d.memberId));
        }


        #region default data generation
        private void generateDefaultData()
        {
            add(new Detail()
                    {
                        memberId = 1,
                        person = new Person()
                                     {
                                         name = new Name()
                                                    {
                                                        firstName = "Hercule",
                                                        lastName = "Poirot",
                                                    },
                                         socialSecurityNumber = "554-98-0001",
                                         dateOfBirth = new DateTime(1876, 4, 1),
                                         gender = Gender.MALE
                                     },
                        address = new Address()
                                      {
                                          line1 = "56B Whitehaven Mansions",
                                          line2 = "Charterhouse Square",
                                          city = "Smithfield London W1",
                                          country = "Great Britain",
                                          state = "England"
                                      },
                        currency = Currency.GPB
                    },
                new Plan()
                    {
                        id = 1,
                        name = "Allpay Total 1000",
                        approvalLimit = 1000
                    }
                );
            add(new Detail()
                    {
                        memberId = 11,
                        person = new Person()
                                     {
                                         name = new Name()
                                                    {
                                                        firstName = "Agatha",
                                                        lastName = "Poirot",
                                                    },
                                         socialSecurityNumber = "554-98-0011",
                                         dateOfBirth = new DateTime(1896, 1, 1),
                                         gender = Gender.FEMALE
                                     },
                        address = new Address()
                                      {
                                          line1 = "56A Whitehaven Mansions",
                                          line2 = "Charterhouse Square",
                                          city = "Smithfield London W1",
                                          country = "Great Britain",
                                          state = "England"
                                      },
                        currency = Currency.GPB
                    },
                new Plan()
                    {
                        id = 1,
                        name = "Allpay Total 1000",
                        approvalLimit = 1000
                    }
                );
            add(new Detail()
                    {
                        memberId = 2,
                        person = new Person()
                                     {
                                         name = new Name()
                                                    {
                                                        firstName = "Sherlock",
                                                        lastName = "Holmes",
                                                    },
                                         socialSecurityNumber = "332-10-0002",
                                         dateOfBirth = new DateTime(1854, 1, 6),
                                         gender = Gender.MALE
                                     },
                        address = new Address()
                                      {
                                          line1 = "221b Baker Street",
                                          city = "London NW1 6XE",
                                          country = "Great Britain",
                                          state = "England"
                                      },
                        currency = Currency.GPB
                    },
                new Plan()
                    {
                        id = 2,
                        name = "Allpay Select 500",
                        approvalLimit = 500
                    }
                );
            add(new Detail()
                    {
                        memberId = 3,
                        person = new Person()
                                     {
                                         name = new Name()
                                                    {
                                                        firstName = "Albert",
                                                        lastName = "Einstein",
                                                    },
                                         socialSecurityNumber = "809-42-0002",
                                         dateOfBirth = new DateTime(1879, 3, 14),
                                         gender = Gender.MALE
                                     },
                        address = new Address()
                                      {
                                          line1 = "3875 Vicksburg Terrace",
                                          city = "Colorado Springs",
                                          country = "USA",
                                          state = "CO",
                                          zip = "80917"
                                      },
                        currency = Currency.USD
                    },
                new Plan()
                    {
                        id = 3,
                        name = "Saver 100",
                        approvalLimit = 100
                    }
                );
        }
        #endregion

    }

}
