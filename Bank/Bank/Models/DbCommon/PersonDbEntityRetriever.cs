using Bank.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class PersonDbEntityRetriever
    {
        public BankAppDbContext Db { get; }

        public PersonDbEntityRetriever(BankAppDbContext context)
        {
            Db = context;
        }

        public List<Birth> GetBirths()
        {
            return Db.Births
                .Include(i => i.Location)
                .Include(i => i.Person)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<City> GetCities()
        {
            return Db.Cities
                .Include(i => i.Locations)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Company> GetCompanies()
        {
            return Db.Companies
                .Include(i => i.Location)
                .Include(i => i.Posts)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Disability> GetDisabilities()
        {
            return Db.Disabilities
                .Include(i => i.People)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<IssuingAuthority> GetIssuingAuthorities()
        {
            return Db.IssuingAuthorities
                .Include(i => i.Location)
                .Include(i => i.Passports)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Location> GetLocations()
        {
            return Db.Locations
                .Include(i => i.City)
                .Include(i => i.Companies)
                .Include(i => i.Births)
                .Include(i => i.PersonToLocations)
                .Include(i => i.IssuingAuthorities)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Nationality> GetNationalities()
        {
            return Db.Nationalities
                .Include(i => i.People)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Passport> GetPassports()
        {
            return Db.Passports
                .Include(i => i.IssuingAuthority)
                .Include(i => i.Person)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<Person> GetPeople()
        {
            return Db.People
                .Include(i => i.Birth)
                .Include(i => i.Passport)
                .Include(i => i.PersonToLocations)
                .Include(i => i.Post)
                .Include(i => i.Nationality)
                .Include(i => i.StandardAccounts)
                .Include(i => i.DepositAccounts)
                .OrderBy(i => i.Id)
                .ToList();
        }
        public List<PersonToLocation> GetPersonToLocations()
        {
            return Db.PersonToLocations
                .Include(i => i.Location)
                .Include(i => i.Person)
                .OrderBy(i => i.PersonId)
                .ThenBy(i => i.LocationId)
                .ThenBy(i => i.IsActual)
                .ToList();
        }
        public List<Post> GetPosts()
        {
            return Db.Posts
                .Include(i => i.Company)
                .Include(i => i.People)
                .OrderBy(i => i.Id)
                .ToList();
        }
    }
}
