using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class TestBankAppContext : DbContext
    {       
        public DbSet<City> Cities { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Birth> Births { get; set; }

        //public DbSet<Client> Clients { get; set; }

        //public DbSet<Company> Companies { get; set; }

        //public DbSet<IssuingAuthority> IssuingAuthorities { get; set; }

        //public DbSet<Passport> Passports { get; set; }

        //public DbSet<Person> People { get; set; }

        //public DbSet<Post> Posts { get; set; }

        public TestBankAppContext(DbContextOptions<TestBankAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
