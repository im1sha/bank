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
            var maritalStatuses = new[]
            {   
                new MaritalStatus
                {
                    Name = "в браке"
                },
                new MaritalStatus
                {
                    Name = "не в браке"
                }
            };
            if (!context.MaritalStatuses.Any())
            {
                context.MaritalStatuses.AddRange(maritalStatuses);
                context.SaveChanges();
            }
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
                    Location = locations[2],
                },
            };
            if (!context.Births.Any())
            {
                context.Births.AddRange(births);
                context.SaveChanges();
            }

            // public DbSet<Post> Posts { get; set; }    

            // public DbSet<IssuingAuthority> IssuingAuthorities { get; set; }

            // public DbSet<Passport> Passports { get; set; }


            // public DbSet<Person> People { get; set; }

            //  public DbSet<Client> Clients { get; set; }

        }
    }
}
