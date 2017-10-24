using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HP.SOAQ.ServiceSimulation.Demo {

    public class XMLMemberAccounts {

        #region performance settings
        private static readonly int MEMBER_SEARCH_RESPONSE_TIME = 200;  // + 100ms Shared resource processing time, see SharedDBConnection
        #endregion

        private readonly IDictionary<string, ISet<Member>> membersByFirstName = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersByLastName = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersBySSN = new Dictionary<string, ISet<Member>>();
        private readonly IDictionary<string, ISet<Member>> membersByZipCode = new Dictionary<string, ISet<Member>>();

        public XMLMemberAccounts() {
            generateDefaultData();
        }

        public IXMLMemberAccountsResponse process(IXMLMemberAccountsRequest request) {
            Console.Write("memberSearch(" + request.name.firstName + ", " + request.name.lastName + ", " + request.socialSecurityNumber + ") ");
            Thread.Sleep(MEMBER_SEARCH_RESPONSE_TIME);

            IXMLMemberAccountsResponse response = new IXMLMemberAccountsResponse();
        
            ISet<Member> members = null;

            if (request.socialSecurityNumber != null) {
                membersBySSN.TryGetValue(request.socialSecurityNumber, out members);
            }
            if (members == null && request.name.lastName != null) {
                membersByLastName.TryGetValue(request.name.lastName, out members);
            }
            if (members == null && request.name.firstName != null) {
                membersByFirstName.TryGetValue(request.name.firstName, out members);
            }
            if (members == null && request.zipCode != null) {
                membersByZipCode.TryGetValue(request.zipCode, out members);
            }

            if (members != null) {
                response.member = filterMembers(members.ToArray<Member>(), request.name, request.dateOfBirth, request.socialSecurityNumber, request.zipCode);
                Console.WriteLine("-> " + response.member.Length + " result" + ((response.member.Length == 1) ? "" : "s"));
            } else {
                Console.WriteLine("-> not found");
                response.member = new Member[0];
            }

            return response;
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
                Detail detail = member.detail;

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
                socialSecurityNumber = d.person.socialSecurityNumber,
                detail = d,
                plan = p
            };
             
            addToMultiMap(membersByFirstName, d.person.name.firstName, m);
            addToMultiMap(membersByLastName, d.person.name.lastName, m);
            addToMultiMap(membersBySSN, d.person.socialSecurityNumber, m);
            addToMultiMap(membersByZipCode, d.address.zip, m);
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
