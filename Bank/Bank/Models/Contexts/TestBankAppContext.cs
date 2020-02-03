using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    public class TestBankAppContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        //public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Birth> Births { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<IssuingAuthority> IssuingAuthorities { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Person> People { get; set; }
        //  public DbSet<Client> Clients { get; set; }
        public DbSet<PersonToLocation> PersonToLocations { get; set; }

        public TestBankAppContext(DbContextOptions<TestBankAppContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
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
                .OnDelete(DeleteBehavior.Restrict);
                
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
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Passport)
                .WithOne(t => t.Person)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Birth)
                .WithMany(t => t.People)
                .OnDelete(DeleteBehavior.Restrict);

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
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Nationality)
                .WithMany(t => t.People)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Person>()
            //    .HasOne(p => p.MaritalStatus)
            //    .WithMany(t => t.People)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Person>()
            //    .HasOne(p => p.MaritalStatus)
            //    .WithMany(t => t.People)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Client>()
            //    .HasOne(p => p.Person)
            //    .WithOne(t => t.Client)
            //    .OnDelete(DeleteBehavior.Restrict);          
        }
    }
}
