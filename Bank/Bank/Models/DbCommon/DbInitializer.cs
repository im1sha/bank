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


            var interestAccruals = new[]
            {
                new InterestAccrual { TermInDays = 95, Name = "95 days" },
                new InterestAccrual { TermInDays = 185, Name = "185 days" },
                new InterestAccrual { TermInDays = 370, Name = "370 days" },
            };
            if (!context.InterestAccruals.Any())
            {
                context.InterestAccruals.AddRange(interestAccruals);
                context.SaveChanges();
            }
            var currencies = new[]
            {            
                new Currency { Name = "EUR" },
                new Currency { Name = "USD" },
                new Currency { Name = "BYN" },
            };
            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(currencies);
                context.SaveChanges();
            }
            var legalEntities = new[]
            {
                new LegalEntity { Name = "Development fund" },
                new LegalEntity { Name = "BelAPB.by cashdesk" },
            };
            if (!context.LegalEntities.Any())
            {
                context.LegalEntities.AddRange(legalEntities);
                context.SaveChanges();
            }
            var depositGenerals = new[]
            {
                new DepositGeneral
                {
                    IsRevocable = false,
                    Name = "Stability+",  
                    ReplenishmentAllowed = true,
                    WithCapitalization = false,
                },
                new DepositGeneral
                {
                    IsRevocable = true,
                    Name = "Growth+",
                    ReplenishmentAllowed = true,
                    WithCapitalization = true,                    
                },
                
            };
            if (!context.DepositGenerals.Any())
            {
                context.DepositGenerals.AddRange(depositGenerals);
                context.SaveChanges();
            }
            var depositVariables = new[]
            {
                new DepositVariable
                {
                   Currency = currencies[0],
                   DepositGeneral = depositGenerals[0],                   
                },
                new DepositVariable
                {
                    Currency = currencies[0],
                    DepositGeneral = depositGenerals[1],
                },
                new DepositVariable
                {
                   Currency = currencies[1],
                   DepositGeneral = depositGenerals[0],
                },
                new DepositVariable
                {
                    Currency = currencies[1],
                    DepositGeneral = depositGenerals[1],
                },
                new DepositVariable
                {
                   Currency = currencies[2],
                   DepositGeneral = depositGenerals[0],
                },
                new DepositVariable
                {
                    Currency = currencies[2],
                    DepositGeneral = depositGenerals[1],
                },
            };
            if (!context.DepositVariables.Any())
            {
                context.DepositVariables.AddRange(depositVariables);
                context.SaveChanges();
            }
            var depositCores = new[]
            {
                new DepositCore
                {
                    DepositVariable = depositVariables[0],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 0.2m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[0],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 0.25m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[0],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 0.3m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[1],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 0.25m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[1],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 0.3m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[1],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 0.7m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[2],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 0.25m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[2],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 0.3m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[2],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 0.9m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[3],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 0.5m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[3],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 0.9m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[3],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 1.4m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[4],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 5.8m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[4],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 5.9m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[4],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 6.1m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[5],
                    InterestAccrual = interestAccruals[0],
                    InterestRate = 9m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[5],
                    InterestAccrual = interestAccruals[1],
                    InterestRate = 9.5m
                },
                new DepositCore
                {
                    DepositVariable = depositVariables[5],
                    InterestAccrual = interestAccruals[2],
                    InterestRate = 11.75m
                },
            };
            if (!context.DepositCores.Any())
            {
                context.DepositCores.AddRange(depositCores);
                context.SaveChanges();
            }
            var depositAccounts = new[]
            {
                new DepositAccount
                {
                    DepositCore = depositCores[0],
                    Person = people[0], 
                },
                new DepositAccount
                {
                    DepositCore = depositCores[8],
                    Person = people[0],
                },
                new DepositAccount
                {
                    DepositCore = depositCores[7],
                    Person = people[1],
                },
                new DepositAccount
                {
                    DepositCore = depositCores[17],
                    Person = people[2],
                },
            };
            if (!context.DepositAccounts.Any())
            {
                context.DepositAccounts.AddRange(depositAccounts);
                context.SaveChanges();
            }
            var standardAccount = new[]
            {
                new StandardAccount
                {
                    Person = people[0],                       
                },
                new StandardAccount
                {
                    Person = people[1],
                },
                new StandardAccount
                {
                    Person = people[2],
                },
                new StandardAccount
                {
                    Person = people[3],
                },
                new StandardAccount
                {
                    Person = people[4],
                },
                new StandardAccount
                {
                    LegalEntity = legalEntities[0],
                },
                new StandardAccount
                {
                    LegalEntity = legalEntities[1],
                },
            };
            if (!context.StandardAccounts.Any())
            {
                context.StandardAccounts.AddRange(standardAccount);
                context.SaveChanges();
            }
            
            var accounts = new[]
            {
                new Account
                {
                    DepositAccount = depositAccounts[0],
                    Name = "dep acc#0",
                    Number = "3014000000008",
                    OpenDate = DateTime.Now.AddDays(-55),                   
                },
                new Account
                {
                    DepositAccount = depositAccounts[1],
                    Name = "dep acc#1",
                    Number = "3014000000007",
                    OpenDate = DateTime.Now.AddDays(-20),
                },
                new Account
                {
                    DepositAccount = depositAccounts[2],
                    Name = "dep acc#2",
                    Number = "3014000000006",
                    OpenDate = DateTime.Now.AddDays(-10),
                },
                new Account
                {
                    DepositAccount = depositAccounts[3],
                    Name = "dep acc#3",
                    Number = "3014000000005",
                    OpenDate = DateTime.Now.AddDays(-5),
                },
                #region accs of person#0 
                new Account
                {
                    StandardAccount = standardAccount[0],
                    Name = "st acc#0-1",
                    Number = "9999000000004",
                    OpenDate = DateTime.Now.AddDays(-500),
                },
                new Account
                {
                    StandardAccount = standardAccount[0],
                    Name = "st acc#0-2",
                    Number = "99990000000003",
                    OpenDate = DateTime.Now.AddDays(-600),
                },
                #endregion
                new Account
                {
                    StandardAccount = standardAccount[1],
                    Name = "st acc#1",
                    Number = "9999000000002",
                    OpenDate = DateTime.Now.AddDays(-500),
                },
                new Account
                {
                    StandardAccount = standardAccount[2],
                    Name = "st acc#2",
                    Number = "9999000000002",
                    OpenDate = DateTime.Now.AddDays(-1000),
                },
                new Account
                {
                    StandardAccount = standardAccount[3],
                    Name = "st acc#3",
                    Number = "9999000000001",
                    OpenDate = DateTime.Now.AddDays(-1500),
                },
                new Account
                {
                    StandardAccount = standardAccount[4],
                    Name = "st acc#4",
                    Number = "9999000000000",
                    OpenDate = DateTime.Now.AddDays(-500),
                },
                new Account
                {
                    StandardAccount = standardAccount[5],
                    Name = "st acc#5 = fund",
                    Number = "7327000000009",
                    OpenDate = DateTime.Now.AddDays(-500),
                },
                new Account
                {
                    StandardAccount = standardAccount[6],
                    Name = "st acc#6 = cashdesk",
                    Number = "1010000000010",
                    OpenDate = DateTime.Now.AddDays(-500),
                },
            };
            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(accounts);
                context.SaveChanges();
            }

            var money = new[]
            {
                new Money { Amount= 25m, Currency = currencies[0], DepositVariable = depositVariables[0], },
                new Money { Amount= 25m, Currency = currencies[0], DepositVariable = depositVariables[1], },
                new Money { Amount= 25m, Currency = currencies[1], DepositVariable = depositVariables[2], },
                new Money { Amount= 25m, Currency = currencies[1], DepositVariable = depositVariables[3], },
                new Money { Amount= 50m, Currency = currencies[2], DepositVariable = depositVariables[4], },
                new Money { Amount= 50m, Currency = currencies[2], DepositVariable = depositVariables[5], },

                new Money { Amount = 100m, Currency = currencies[0], Account = accounts[0], },
                new Money { Amount = 100m, Currency = currencies[1], Account = accounts[1], },
                new Money { Amount = 100m, Currency = currencies[1], Account = accounts[2], },
                new Money { Amount = 100m, Currency = currencies[2], Account = accounts[3], },

                new Money { Amount = 100m, Currency = currencies[0], Account = accounts[4], },
                new Money { Amount = 50m, Currency = currencies[1], Account = accounts[5], },
                new Money { Amount = 100m, Currency = currencies[1], Account = accounts[6], },
                new Money { Amount = 100m, Currency = currencies[2], Account = accounts[7], },
                new Money { Amount = 100m, Currency = currencies[2], Account = accounts[8], },
                new Money { Amount = 100m, Currency = currencies[2], Account = accounts[9], },

                new Money { Amount = 1000000000000m, Currency = currencies[2], Account = accounts[10], },
                new Money { Amount = 10m, Currency = currencies[2], Account = accounts[11], },
            };
            if (!context.Moneys.Any())
            {
                context.Moneys.AddRange(money);
                context.SaveChanges();
            }
        }
    }
}
