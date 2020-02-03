using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public static class SampleData
    {
        public static void Initialize(TestBankAppContext context)
        {
            var cities = new[] 
            {
                new City
                {
                    Name = "Минск"
                },
                new City
                {
                    Name = "Гродно"
                },
                new City
                {
                    Name = "Гомель"
                },
                new City
                {
                    Name = "Витебск"
                },
                new City
                {
                    Name = "Брест"
                },
                new City
                {
                    Name = "Могилев"
                }
            };
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(cities);
                context.SaveChanges();
            }
            var disabilities = new[]
            {
                new Disability
                {
                    Name = "нет",
                },
                new Disability
                {
                    Name = "I группа",
                },
                new Disability
                {
                    Name = "II группа"
                },
                new Disability
                {
                    Name = "III группа"
                } 
            };
            if (!context.Disabilities.Any())
            {
                context.Disabilities.AddRange(disabilities);
                context.SaveChanges();
            }
            //var maritalStatuses = new[]
            //{   
            //    new MaritalStatus
            //    {
            //        Name = "не в браке"
            //    },
            //    new MaritalStatus
            //    {
            //        Name = "в браке"
            //    },               
            //};
            //if (!context.MaritalStatuses.Any())
            //{
            //    context.MaritalStatuses.AddRange(maritalStatuses);
            //    context.SaveChanges();
            //}
            var nationalities = new[]
            {
                new Nationality
                {
                    Name = "белорус"
                },
                new Nationality
                {
                    Name = "англичанин"
                },
                new Nationality
                {
                    Name = "поляк"
                },
                new Nationality
                {
                    Name = "украинец"
                },
                new Nationality
                {
                    Name = "немец"
                }
            };
            if (!context.Nationalities.Any())
            {
                context.Nationalities.AddRange(nationalities);
                context.SaveChanges();
            }
            var locations = new[]
            {
                new Location
                {
                    City = cities[0],
                    BuildingNumber = "1",
                    Street = "Центральная",
                },
                new Location
                {
                    City = cities[1],
                    BuildingNumber = "22",
                    Street = "Советская",
                },
                new Location
                {
                    City = cities[2],
                    BuildingNumber = "33",
                    Street = "Победителей",
                },
                new Location
                {
                    City = cities[3],
                    BuildingNumber = "51",
                    Street = "Народная",
                },
                new Location
                {
                    City = cities[4],
                    BuildingNumber = "12",
                    Street = "Мира",
                },
            };
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(locations);
                context.SaveChanges();
            }
            var births = new[]
            {
                new Birth
                {
                    Date = new DateTime(2000, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Date = new DateTime(1999, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Date = new DateTime(1998, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Date = new DateTime(1997, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Date = new DateTime(1996, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Date = new DateTime(1995, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Date = new DateTime(1994, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Date = new DateTime(1993, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Date = new DateTime(1992, 1, 2),
                    Location = locations[2],
                },
                new Birth
                {
                    Date = new DateTime(1991, 1, 2),
                    Location = locations[3],
                },
            };
            if (!context.Births.Any())
            {
                context.Births.AddRange(births);
                context.SaveChanges();
            }
            var companies = new[]
            {
                new Company
                {
                   Name = "Company 1" ,
                   Location = locations[0]
                },
                new Company
                {
                   Name = "Company 2" ,
                   Location = locations[1]
                },
                new Company
                {
                   Name = "Company 3" ,
                   Location = locations[2]
                },
                new Company
                {
                   Name = "Company 4" ,
                   Location = locations[3]
                },
            };
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(companies);
                context.SaveChanges();
            }
            var posts = new[]
            {
                new Post
                {
                    Company = companies[0],
                    Name = "CEO"
                },
                new Post
                {
                    Company = companies[1],
                    Name = "HR"
                },
                new Post
                {
                    Company = companies[1],
                    Name = "JS developer"
                },
                new Post
                {
                    Company = companies[2],
                    Name = "Full-stack developer"
                },
            };
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(posts);
                context.SaveChanges();
            }
            var issuingAuthorithies = new[]
            {
                new IssuingAuthority
                {
                    Name = "РОВД Заводского района",
                    Location = locations[0],
                },
                new IssuingAuthority
                {
                    Name = "РОВД Фрунзенского района",
                    Location = locations[1],
                },
                new IssuingAuthority
                {
                    Name = "РОВД Центрального района",
                    Location = locations[2],
                },
            };
            if (!context.IssuingAuthorities.Any())
            {
                context.IssuingAuthorities.AddRange(issuingAuthorithies);
                context.SaveChanges();
            }
            var passports = new[]
            {
                new Passport
                {
                    IdentifyingNumber = "11111111111111111",
                    Series = "AA",
                    Number = "1111",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                },
                new Passport
                {
                    IdentifyingNumber = "22222222222222222",
                    Series = "AA",
                    Number = "2222",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                },
                new Passport
                {
                    IdentifyingNumber = "33333333333333333",
                    Series = "AA",
                    Number = "3333",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                },
                new Passport
                {
                    IdentifyingNumber = "44444444444444444",
                    Series = "AA",
                    Number = "4444",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                },
                new Passport
                {
                    IdentifyingNumber = "5555555555555555",
                    Series = "AA",
                    Number = "5555",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                },
                new Passport
                {
                    IdentifyingNumber = "666666666666666",
                    Series = "AA",
                    Number = "6666",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[2],
                },
                new Passport
                {
                    IdentifyingNumber = "7777777777777777",
                    Series = "AA",
                    Number = "7777",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[2],
                },
                new Passport
                {
                    IdentifyingNumber = "88888888888888888",
                    Series = "AA",
                    Number = "8888",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                },
                new Passport
                {
                    IdentifyingNumber = "99999999999999999",
                    Series = "AA",
                    Number = "9999",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                },
                new Passport
                {
                    IdentifyingNumber = "111111122222222222",
                    Series = "AA",
                    Number = "1122",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                },
                new Passport
                {
                    IdentifyingNumber = "11111111333333333333",
                    Series = "AA",
                    Number = "1133",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                },
            };
            if (!context.Passports.Any())
            {
                context.Passports.AddRange(passports);
                context.SaveChanges();
            }
            var people = new[]
            {
                new Person
                {
                    Birth = births[0],
                    Disability = disabilities[0],
                    Email = "email0@mail.com",
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    HomePhone = "1111111",
                    MobilePhone = "2111111",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Passport = passports[0],
                    Post = posts[0],
                    Revenue = 2000000,
                },
                new Person
                {
                    Birth = births[1],
                    Disability = disabilities[0],
                    Email = "email111111111@mail.com",
                    FirstName = "Петр",
                    LastName = "Петров",
                    MiddleName = "Иванович",
                    HomePhone = "1222222",
                    MobilePhone = "2222222",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Passport = passports[1],
                    Post = posts[1],
                    Revenue = 100000,
                },
                new Person
                {
                    Birth = births[2],
                    Disability = disabilities[0],
                    Email = "email2@mail.com",
                    FirstName = "Валентин",
                    LastName = "Михайлов",
                    MiddleName = "Михайлович",
                    HomePhone = "133333333",
                    MobilePhone = "23333333",
                    IsPensioner = false,
                    MaritalStatus = true,
                    Nationality = nationalities[0],
                    Passport = passports[2],
                    Post = posts[1],
                    Revenue = 150000,
                },
                new Person
                {
                    Birth = births[3],
                    Disability = disabilities[1],
                    Email = "email3@mail.com",
                    FirstName = "Анна",
                    LastName = "Петрова",
                    MiddleName = "Юрьевна",
                    HomePhone = "1444444",
                    MobilePhone = "2444444",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[3],
                    Passport = passports[3],
                    Post = posts[2],
                    Revenue = 350000,
                },
                new Person
                {
                    Birth = births[4],
                    Disability = disabilities[0],
                    Email = "email4@mail.com",
                    FirstName = "Сидорова",
                    LastName = "Маргарита",
                    MiddleName = "Семеновна",
                    HomePhone = "155555",
                    MobilePhone = "2555555",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Passport = passports[4],
                    Post = posts[3],
                    Revenue = 700000,
                },        
            };
            if (!context.People.Any())
            {
                context.People.AddRange(people);
                context.SaveChanges();
            }

            var peopleToLocations = new[]
            {
                new PersonToLocation
                { 
                    IsActual = true,
                    Location = locations[0],
                    Person = people[0]
                },
                new PersonToLocation
                {
                    IsActual = false,
                    Location = locations[0],
                    Person = people[0]
                },
                new PersonToLocation
                {
                    IsActual = true,
                    Location = locations[0],
                    Person = people[1]
                },
                new PersonToLocation
                {
                    IsActual = false,
                    Location = locations[1],
                    Person = people[1]
                },
                new PersonToLocation
                {
                    IsActual = true,
                    Location = locations[2],
                    Person = people[2]
                },
                new PersonToLocation
                {
                    IsActual = false,
                    Location = locations[2],
                    Person = people[2]
                },
                new PersonToLocation
                {
                    IsActual = true,
                    Location = locations[0],
                    Person = people[3]
                },
                new PersonToLocation
                {
                    IsActual = false,
                    Location = locations[2],
                    Person = people[3]
                }, 
                new PersonToLocation
                {
                    IsActual = true,
                    Location = locations[1],
                    Person = people[4]
                },
                new PersonToLocation
                {
                    IsActual = false,
                    Location = locations[0],
                    Person = people[4]
                },
            };
            if (!context.PersonToLocations.Any())
            {
                for (int i = 0; i < peopleToLocations.Length; i++)
                {
                    try
                    {
                        context.PersonToLocations.Add(peopleToLocations[i]);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }              
                context.SaveChanges();
            }


            //if (!context.Clients.Any())
            //{
            //    context.Clients.AddRange(people.Select(i => new Client { Person = i, }));
            //    context.SaveChanges();
            //}
        }
    }
}
