using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    public class BankAppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Disability> Disabilities { get; set; }

        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Birth> Births { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<IssuingAuthority> IssuingAuthorities { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<PersonToLocation> PersonToLocations { get; set; }

        public DbSet<InterestAccrual> InterestAccruals { get; set; }

        public DbSet<DepositGeneral> DepositGenerals { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<DepositVariable> DepositVariables { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<DepositAccount> DepositAccounts { get; set; }

        public DbSet<StandardAccount> StandardAccounts { get; set; }

        public DbSet<Log> Logs { get; set; }

        public BankAppDbContext(DbContextOptions<BankAppDbContext> options)
            : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasOne(p => p.City)
                .WithMany(t => t.Locations)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Birth>()
                .HasOne(p => p.Location)
                .WithMany(t => t.Births)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IssuingAuthority>()
                .HasOne(p => p.Location)
                .WithMany(t => t.IssuingAuthorities)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasOne(p => p.Location)
                .WithMany(t => t.Companies)
                .OnDelete(DeleteBehavior.Restrict);

            #region m2m

            modelBuilder.Entity<PersonToLocation>()
                .HasKey(u => new { u.PersonId, u.IsActual });

            modelBuilder.Entity<PersonToLocation>()
                .HasOne(p => p.Person)
                .WithMany(t => t.PersonToLocations)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<PersonToLocation>()
                .HasOne(p => p.Location)
                .WithMany(t => t.PersonToLocations)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            modelBuilder.Entity<Passport>()
                .HasOne(p => p.IssuingAuthority)
                .WithMany(t => t.Passports)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 to 1
            modelBuilder.Entity<Passport>()
                .HasOne(p => p.Person)
                .WithOne(t => t.Passport)
                .OnDelete(DeleteBehavior.Cascade);            

            modelBuilder.Entity<Birth>()
                .HasOne(p => p.Person)
                .WithOne(t => t.Birth)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Company)
                .WithMany(t => t.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Post)
                .WithMany(t => t.People)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Disability)
                .WithMany(t => t.People)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Nationality)
                .WithMany(t => t.People)
                .OnDelete(DeleteBehavior.Restrict);

            #region deposit

            modelBuilder.Entity<DepositGeneral>()
                .HasOne(p => p.InterestAccrual)
                .WithMany(t => t.DepositGenerals)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepositVariable>()
                .HasOne(p => p.DepositGeneral)
                .WithMany(t => t.DepositVariables)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DepositVariable>()
                .HasOne(p => p.Currency)
                .WithMany(t => t.DepositVariables)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepositAccount>()
                .HasOne(p => p.DepositVariable)
                .WithMany(t => t.DepositAccounts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepositAccount>()
                .HasOne(p => p.Person)
                .WithMany(t => t.DepositAccounts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StandardAccount>()
                .HasOne(p => p.LegalEntity)
                .WithMany(t => t.StandardAccounts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StandardAccount>()
                .HasOne(p => p.Person)
                .WithMany(t => t.StandardAccounts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(p => p.DepositAccount)
                .WithOne(t => t.Account)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasOne(p => p.StandardAccount)
                .WithOne(t => t.Account)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Log>()
                .HasOne(p => p.Account)
                .WithMany(t => t.Logs)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
