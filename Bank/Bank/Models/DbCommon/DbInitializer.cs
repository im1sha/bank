using System.Linq;

namespace Bank.Models
{
    public static class DbInitializer
    {
        public static void Initialze(BankAppDbContext context, TimeService timeService)
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
                    Date = timeService.CurrentTime.AddYears(-20),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[1],
                    Date = timeService.CurrentTime.AddYears(-21),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[2],
                    Date = timeService.CurrentTime.AddYears(-22),
                    Location = locations[0],
                },
                new Birth
                {
                    Person = people[3],
                    Date = timeService.CurrentTime.AddYears(-23),
                    Location = locations[1],
                },
                new Birth
                {
                    Person = people[4],
                    Date = timeService.CurrentTime.AddYears(-24),
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
                    IssuingDate = timeService.CurrentTime.AddYears(-2),
                    IssuingAuthority = issuingAuthorithies[0],
                    Person = people[0]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA2",
                    Series = "AA",
                    Number = "2222222",
                    IssuingDate = timeService.CurrentTime.AddYears(-1),
                    IssuingAuthority = issuingAuthorithies[1],
                    Person = people[1]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA3",
                    Series = "AA",
                    Number = "3333333",
                    IssuingDate = timeService.CurrentTime.AddYears(-2),
                    IssuingAuthority = issuingAuthorithies[0],
                    Person = people[2]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA4",
                    Series = "AA",
                    Number = "4444444",
                    IssuingDate = timeService.CurrentTime.AddYears(-2),
                    IssuingAuthority = issuingAuthorithies[1],
                    Person = people[3]
                },
                new Passport
                {
                    IdentifyingNumber = "1111111A123AA5",
                    Series = "AA",
                    Number = "5555555",
                    IssuingDate = timeService.CurrentTime.AddYears(-3),
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

            //=================================================================

            var interestAccruals = new[]
            {
                new InterestAccrual { TermInDays = 95, Name = "95 days" },
                new InterestAccrual { TermInDays = 185, Name = "185 days" },
                new InterestAccrual { TermInDays = 370, Name = "370 days" },
                new InterestAccrual { TermInDays = 540, Name = "18 month (540 days)" },
                new InterestAccrual { TermInDays = 1800, Name = "5 years (1800 days)" },
                new InterestAccrual { TermInDays = 3600, Name = "10 years (3600 days)" },
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

            var money = new[]
            {
                // deposit terms min money amount : DepositVariable
                new Money { Amount= 25m, Currency = currencies[0], },//DepositVariable = depositVariables[0], }, //0
                new Money { Amount= 25m, Currency = currencies[0], },//DepositVariable = depositVariables[1], }, //1
                new Money { Amount= 25m, Currency = currencies[1], },//DepositVariable = depositVariables[2], }, //2
                new Money { Amount= 25m, Currency = currencies[1], },//DepositVariable = depositVariables[3], }, //3
                new Money { Amount= 50m, Currency = currencies[2], },//DepositVariable = depositVariables[4], }, //4
                new Money { Amount= 50m, Currency = currencies[2], },//DepositVariable = depositVariables[5], }, //5


                // deposits : Account + DepositAccount
                new Money { Amount = 100m, Currency = currencies[0], },//Account = accounts[0], },//6
                new Money { Amount = 100m, Currency = currencies[1], },//Account = accounts[1], },//7
                new Money { Amount = 100m, Currency = currencies[1], },//Account = accounts[2], },//8
                new Money { Amount = 100m, Currency = currencies[2], },//Account = accounts[3], },//9

                new Money { Amount = 0, Currency = currencies[0], },// DepositAccount = accounts[0].DepositAccount, },//10
                new Money { Amount = 0, Currency = currencies[1], },//DepositAccount = accounts[1].DepositAccount, },//11
                new Money { Amount = 0, Currency = currencies[1], },//DepositAccount = accounts[2].DepositAccount, },//12
                new Money { Amount = 0, Currency = currencies[2], },//DepositAccount = accounts[3].DepositAccount, },//13


                // standard'DevFund related : Account
                new Money { Amount = 10000000000m, Currency = currencies[0], },//Account = accounts[4], },//14
                new Money { Amount = 20000000000m, Currency = currencies[1], },//Account = accounts[5], },//15
                new Money { Amount = 30000000000m, Currency = currencies[2], },//Account = accounts[6], },//16
                // standard related : Account
                new Money { Amount = 50m,  Currency = currencies[0], },//Account = accounts[7], },//17
                new Money { Amount = 100m, Currency = currencies[1], },//Account = accounts[8], },
                new Money { Amount = 100m, Currency = currencies[2], },//Account = accounts[9], },
                new Money { Amount = 100m, Currency = currencies[2], },//Account = accounts[10], },
              

                // credits : CreditTerm.MinimalCredit
                new Money { Amount = 1000m, Currency = currencies[2], }, //21
                new Money { Amount = 1000m, Currency = currencies[2], },
                new Money { Amount = 100m, Currency = currencies[2], },
                //credits : CreditTerm.MaximalCredit
                new Money { Amount = 1000000m, Currency = currencies[2], }, //24
                new Money { Amount = 1000000m, Currency = currencies[2], },
                new Money { Amount = 1000000m, Currency = currencies[2], },

                // credits : Account
                new Money { Amount = 1000m, Currency = currencies[2], }, //27
                new Money { Amount = 2000m, Currency = currencies[2], },
                new Money { Amount = 3000m, Currency = currencies[2], },

                // credits : CreditAccount for Account with Money[27]
                new Money {Amount = 0m, Currency = currencies[2], },//30
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                // credits : CreditAccount for Account with Money[28]
                new Money {Amount = 0m, Currency = currencies[2], },//34
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                // credits : CreditAccount for Account with Money[29]
                new Money {Amount = 0m, Currency = currencies[2], },//38
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
               
                //credits : CreditAccount .Percentage
                new Money {Amount = 0m, Currency = currencies[2], },//42
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
                new Money {Amount = 0m, Currency = currencies[2], },
            };
            if (!context.Moneys.Any())
            {
                context.Moneys.AddRange(money);
                context.SaveChanges();
            }

            var legalEntities = new[]
            {
                new LegalEntity { Name = "Development fund BelAPB.by" },
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
                    ReplenishmentAllowed = false,
                    WithCapitalization = false,
                },
                new DepositGeneral
                {
                    IsRevocable = true,
                    Name = "Growth+",
                    ReplenishmentAllowed = false,
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
                   MinimalDeposit = money[0],
                },
                new DepositVariable
                {
                    Currency = currencies[0],
                    DepositGeneral = depositGenerals[1],
                    MinimalDeposit = money[1],
                },
                new DepositVariable
                {
                   Currency = currencies[1],
                   DepositGeneral = depositGenerals[0],
                   MinimalDeposit = money[2],
                },
                new DepositVariable
                {
                    Currency = currencies[1],
                    DepositGeneral = depositGenerals[1],
                    MinimalDeposit = money[3],

                },
                new DepositVariable
                {
                   Currency = currencies[2],
                   DepositGeneral = depositGenerals[0],
                   MinimalDeposit = money[4],
                },
                new DepositVariable
                {
                    Currency = currencies[2],
                    DepositGeneral = depositGenerals[1],
                    MinimalDeposit = money[5],
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
                    Profit = money[10],
                },
                new DepositAccount
                {
                    DepositCore = depositCores[8],
                    Person = people[0],
                    Profit = money[11],
                },
                new DepositAccount
                {
                    DepositCore = depositCores[7],
                    Person = people[1],
                    Profit = money[12],
                },
                new DepositAccount
                {
                    DepositCore = depositCores[17],
                    Person = people[2],
                    Profit = money[13],
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
                    LegalEntity = legalEntities[0],
                },
                new StandardAccount
                {
                    LegalEntity = legalEntities[0],
                },
                new StandardAccount
                {
                    LegalEntity = legalEntities[0],
                },
            };
            if (!context.StandardAccounts.Any())
            {
                context.StandardAccounts.AddRange(standardAccount);
                context.SaveChanges();
            }

            var creditTerms = new[]
            {
                new CreditTerm
                {
                    Name = "Auto (18 month)",
                    Currency = currencies[2],
                    DailyFineRate = 0.15m,
                    EarlyRepaymentAllowed = false,
                    InterestAccrual = interestAccruals[3],
                    InterestRate = 3.9m,
                    IsAnnuity = true,
                    MaximalCredit = money[24],
                    MinimalCredit = money[21],
                },
                new CreditTerm
                {
                    Name = "Auto (10 years)",
                    Currency = currencies[2],
                    DailyFineRate = 0.15m,
                    EarlyRepaymentAllowed = false,
                    InterestAccrual = interestAccruals[5],
                    InterestRate = 12.32m,
                    IsAnnuity = true,
                    MaximalCredit = money[25],
                    MinimalCredit = money[22],
                },
                new CreditTerm
                {
                    Name = "Products",
                    Currency = currencies[2],
                    DailyFineRate = 0.15m,
                    EarlyRepaymentAllowed = false,
                    InterestAccrual = interestAccruals[4],
                    InterestRate = 12.32m,
                    IsAnnuity = false,
                    MaximalCredit = money[26],
                    MinimalCredit = money[23],
                },
            };

            var creditAccounts = new[]
            {
                new CreditAccount
                {
                    CreditTerm = creditTerms[0],
                    Fine = money[30],
                    PaidFinePart=money[31],
                    PaidMainPart=money[32],
                    PaidPercentagePart=money[33],
                    Percentage= money[42],
                    Person = people[0],
                },
                new CreditAccount
                {
                    CreditTerm = creditTerms[1],
                    Fine = money[34],
                    PaidFinePart=money[35],
                    PaidMainPart=money[36],
                    PaidPercentagePart=money[37],
                    Percentage= money[43],
                    Person = people[1],
                },
                new CreditAccount
                {
                    CreditTerm = creditTerms[2],
                    Fine = money[38],
                    PaidFinePart=money[39],
                    PaidMainPart=money[40],
                    PaidPercentagePart=money[41],
                    Percentage= money[44],
                    Person = people[2],
                },
            };

            var accounts = new[]
            {
                #region accounts for deposit accounts

                new Account
                {
                    DepositAccount = depositAccounts[0],
                    Name = "dep acc#0",
                    Number = "3014000000008",
                    OpenDate = timeService.CurrentTime,
                    Money = money[6],
                },
                new Account
                {
                    DepositAccount = depositAccounts[1],
                    Name = "dep acc#1",
                    Number = "3014000000007",
                    OpenDate = timeService.CurrentTime,
                    Money = money[7],
                },
                new Account
                {
                    DepositAccount = depositAccounts[2],
                    Name = "dep acc#2",
                    Number = "3014000000006",
                    OpenDate = timeService.CurrentTime,
                    Money = money[8],
                },
                new Account
                {
                    DepositAccount = depositAccounts[3],
                    Name = "dep acc#3",
                    Number = "3014000000005",
                    OpenDate = timeService.CurrentTime,
                    Money = money[9],
                },

                #endregion

                #region for development fund

                new Account
                {
                    StandardAccount = standardAccount[4],
                    Name = "st acc#4 = fund",
                    Number = "7327000000009",
                    OpenDate = timeService.CurrentTime.AddDays(-500),
                    Money = money[14],
                },
                new Account
                {
                    StandardAccount = standardAccount[5],
                    Name = "st acc#5 = fund",
                    Number = "7327000000010",
                    OpenDate = timeService.CurrentTime.AddDays(-500),
                    Money = money[15],
                },
                new Account
                {
                    StandardAccount = standardAccount[6],
                    Name = "st acc#6 = fund",
                    Number = "7327000000011",
                    OpenDate = timeService.CurrentTime.AddDays(-500),
                    Money = money[16],
                },

                #endregion   

                #region accs of person standard accounts

                new Account
                {
                    StandardAccount = standardAccount[0],
                    Name = "st acc#0",
                    Number = "9999000000004",
                    OpenDate = timeService.CurrentTime.AddDays(-500),
                    Money = money[17],
                },
                new Account
                {
                    StandardAccount = standardAccount[1],
                    Name = "st acc#1",
                    Number = "9999000000003",
                    OpenDate = timeService.CurrentTime.AddDays(-500),
                    Money = money[18],
                },
                new Account
                {
                    StandardAccount = standardAccount[2],
                    Name = "st acc#2",
                    Number = "9999000000002",
                    OpenDate = timeService.CurrentTime.AddDays(-1000),
                    Money = money[19],
                },
                new Account
                {
                    StandardAccount = standardAccount[3],
                    Name = "st acc#3",
                    Number = "9999000000001",
                    OpenDate = timeService.CurrentTime.AddDays(-1500),
                    Money = money[20],
                },

                #endregion

                #region accounts for credits

                new Account
                {
                    CreditAccount = creditAccounts[0],
                    Name = "credit acc#1",
                    Number = "1111000000001",
                    OpenDate = timeService.CurrentTime,
                    Money = money[27],
                },
                new Account
                {
                    CreditAccount = creditAccounts[1],
                    Name = "credit acc#2",
                    Number = "1111000000002",
                    OpenDate = timeService.CurrentTime,
                    Money = money[28],
                },
                new Account
                {
                    CreditAccount = creditAccounts[2],
                    Name = "credit acc#3",
                    Number = "1111000000003",
                    OpenDate = timeService.CurrentTime,
                    Money = money[29],
                }

                #endregion
            };
            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(accounts);
                context.SaveChanges();
            }
        }
    }
}


