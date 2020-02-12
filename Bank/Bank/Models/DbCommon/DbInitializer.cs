using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public static class DbInitializer
    {
        public static void InitializePeopleAndRelatedEntities(BankAppDbContext context)
        {
            var cities = new[] 
            {
                new City
                {
                    Name = "Minsk"
                },
                new City
                {
                    Name = "Grodno"
                },
                new City
                {
                    Name = "Gomel"
                },
                new City
                {
                    Name = "Vitebsk"
                },
                new City
                {
                    Name = "Brest"
                },
                new City
                {
                    Name = "Mogilev"
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
                    Name = "No",
                },
                new Disability
                {
                    Name = "I group",
                },
                new Disability
                {
                    Name = "II group"
                },
                new Disability
                {
                    Name = "III group"
                } 
            };
            if (!context.Disabilities.Any())
            {
                context.Disabilities.AddRange(disabilities);
                context.SaveChanges();
            }          
            var nationalities = new[]
            {
                new Nationality
                {
                    Name = "Belarus"
                },
                new Nationality
                {
                    Name = "UK"
                },
                new Nationality
                {
                    Name = "Poland"
                },
                new Nationality
                {
                    Name = "Ukraine"
                },
                new Nationality
                {
                    Name = "Germany"
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
                    Street = "A",
                },
                new Location
                {
                    City = cities[1],
                    BuildingNumber = "22",
                    Street = "B",
                },
                new Location
                {
                    City = cities[2],
                    BuildingNumber = "33",
                    Street = "C",
                },
                new Location
                {
                    City = cities[3],
                    BuildingNumber = "51",
                    Street = "D",
                },
                new Location
                {
                    City = cities[4],
                    BuildingNumber = "12",
                    Street = "E",
                },
            };
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(locations);
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
                    Name = ".NET developer"
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
                    Name = "Issuing Authority 1",
                    Location = locations[0],
                },
                new IssuingAuthority
                {
                    Name = "Issuing Authority 2",
                    Location = locations[1],
                },
                new IssuingAuthority
                {
                    Name = "Issuing Authority 3",
                    Location = locations[2],
                },
            };
            if (!context.IssuingAuthorities.Any())
            {
                context.IssuingAuthorities.AddRange(issuingAuthorithies);
                context.SaveChanges();
            }          
            var people = new[]
            {
                new Person
                {
                    Disability = disabilities[0],
                    Email = "email0@mail.com",
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    MiddleName = "Ivanovich",
                    HomePhone = "375171234567",
                    MobilePhone = "375291234567",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Post = posts[0],
                    Revenue = 2000000,                   
                },
                new Person
                {
                    Disability = disabilities[0],
                    Email = "abcd@mail.com",
                    FirstName = "Peter",
                    LastName = "Petrov",
                    MiddleName = "Ivanovich",
                    HomePhone = "375171234566",
                    MobilePhone = "375291234566",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Post = posts[1],
                    Revenue = 100000,
                },
                new Person
                {
                    Disability = disabilities[0],
                    Email = "ui@aaa.com",
                    FirstName = "Valentin",
                    LastName = "Mikhailov",
                    MiddleName = "Mikhailovich",
                    HomePhone = "375171234565",
                    MobilePhone = "375291234565",
                    IsPensioner = false,
                    MaritalStatus = true,
                    Nationality = nationalities[0],
                    Post = posts[1],
                    Revenue = 150000,
                },
                new Person
                {
                    Disability = disabilities[1],
                    Email = "email3@mail.com",
                    FirstName = "Ann",
                    LastName = "Petrova",
                    MiddleName = "Anatolyevna",
                    HomePhone = "375171234564",
                    MobilePhone = "375291234564",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[3],
                    Post = posts[2],
                    Revenue = 350000,
                },
                new Person
                {
                    Disability = disabilities[0],
                    Email = "email4@mail.com",
                    FirstName = "Margarita",
                    LastName = "Sidorova",
                    MiddleName = "Semyonovna",
                    HomePhone = "375171234563",
                    MobilePhone = "375291234563",
                    IsPensioner = false,
                    MaritalStatus = false,
                    Nationality = nationalities[0],
                    Post = posts[3],
                    Revenue = 700000,
                },        
            };
            if (!context.People.Any())
            {
                context.People.AddRange(people);
                context.SaveChanges();
            }
            var births = new[]
           {
                new Birth
                {
                    Person = people[0],
                    Date = new DateTime(2000, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[1],
                    Date = new DateTime(1999, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[2],
                    Date = new DateTime(1998, 1, 2),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[3],
                    Date = new DateTime(1997, 1, 2),
                    Location = locations[1],
                },
                new Birth
                {
                    Person = people[4],
                    Date = new DateTime(1996, 1, 2),
                    Location = locations[1],
                },               
            };
            if (!context.Births.Any())
            {
                context.Births.AddRange(births);
                context.SaveChanges();
            }
            var passports = new[]
            {
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA1",
                    Series = "AA",
                    Number = "1111111",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                    Person = people[0]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA2",
                    Series = "AA",
                    Number = "2222222",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                    Person = people[1]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA3",
                    Series = "AA",
                    Number = "3333333",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                    Person = people[2]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA4",
                    Series = "AA",
                    Number = "4444444",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[1],
                    Person = people[3]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA5",
                    Series = "AA",
                    Number = "5555555",
                    IssuingDate = new DateTime(2016, 1, 1),
                    IssuingAuthority = issuingAuthorithies[0],
                    Person = people[4]
                },           
            };
            if (!context.Passports.Any())
            {
                context.Passports.AddRange(passports);
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
                    context.PersonToLocations.Add(peopleToLocations[i]);                    
                }              
                context.SaveChanges();
            }       
        }

        public static void InitializeDepositsAndRelatedEntities(BankAppDbContext context)
        {
            //var cities = new[]
            //{
                
            //};
            //if (!context.Cities.Any())
            //{
            //    context.Cities.AddRange(cities);
            //    context.SaveChanges();
            //}
        }
    }
}
