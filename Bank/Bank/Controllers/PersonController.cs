using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bank.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly BankAppDbContext _db;

        public PersonController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            _db = context;
            _logger = logger;
        }

        private List<Person> RetreivePeople()
        {
            return _db.People
                .Include(u => u.Birth).ThenInclude(u => u.Location).ThenInclude(u => u.City)
                .Include(u => u.Disability)
                .Include(u => u.Nationality)
                .Include(u => u.Passport).ThenInclude(u => u.IssuingAuthority)
                .Include(u => u.PersonToLocations).ThenInclude(u => u.Location).ThenInclude(u => u.City)
                .Include(u => u.Post).ThenInclude(u => u.Company)
                .ToList();
        }

        private List<City> GetCities()
        {
            return _db.Cities.OrderBy(i => i.Id).ToList();
        }

        private List<(string Series, string Number, string IdentifyingNumber, int PersonId)> GetPassports()
        {
            var series = _db.Passports.OrderBy(i => i.IdentifyingNumber).Select(i => i.Series).ToList();
            var numbers = _db.Passports.OrderBy(i => i.IdentifyingNumber).Select(i => i.Number).ToList();
            var ids = _db.Passports.OrderBy(i => i.IdentifyingNumber).Select(i => i.IdentifyingNumber).ToList();
            var personIds = _db.Passports.OrderBy(i => i.IdentifyingNumber).Select(i => i.PersonId).ToList();

            if (series.Count() != numbers.Count() || numbers.Count() != ids.Count() || numbers.Count() != personIds.Count())
            {
                throw new ApplicationException();
            }
            List<(string Series, string Number, string IdentifyingNumber, int PersonId)> result
                = new List<(string Series, string Number, string IdentifyingNumber, int PersonId)>();

            foreach ((((string ser, string num), string id), int pers) in series.Zip(numbers).Zip(ids).Zip(personIds))
            {
                result.Add((ser, num, id, pers));
            }

            return result;
        }

        private List<Nationality> GetNationalities()
        {
            return _db.Nationalities.OrderBy(i => i.Id).ToList();
        }

        private static readonly List<MaritalStatusLocal> _maritalStatusLocals =
            new List<MaritalStatusLocal>()
            {
                new MaritalStatusLocal { Id = 1, Name = "Yes", BoolValue = true },
                new MaritalStatusLocal { Id = 2, Name = "No", BoolValue = false }
            };

        private List<MaritalStatusLocal> GetMaritalStatuses()
        {
            return _maritalStatusLocals;
        }

        private MaritalStatusLocal GetMaritalStatus(bool status)
        {
            return GetMaritalStatuses().First(i => i.BoolValue == status);
        }

        private List<Disability> GetDisabilities()
        {
            return _db.Disabilities.OrderBy(i => i.Id).ToList();
        }

        private List<PersonFullViewModel> ConvertPeopleToPersonFullViewModels(List<Person> people)
        {
            List<PersonFullViewModel> result = new List<PersonFullViewModel>();

            foreach (var item in people)
            {
                var actualLocation = item.PersonToLocations.FirstOrDefault(i => i.PersonId == item.Id && i.IsActual);
                var residenceLocation = item.PersonToLocations.FirstOrDefault(i => i.PersonId == item.Id && !i.IsActual);

                result.Add(new PersonFullViewModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    MiddleName = item.MiddleName,
                    PassportSeries = item.Passport.Series,
                    PassportNumber = item.Passport.Number,
                    PassportIdentifyingNumber = item.Passport.IdentifyingNumber,
                    PassportIssuingAuthorityId = item.Passport.IssuingAuthorityId,
                    PassportIssuingAuthorityName = item.Passport.IssuingAuthority.Name,
                    PassportIssuingDate = item.Passport.IssuingDate,
                    BirthDate = item.Birth.Date,
                    BirthLocationId = item.Birth.LocationId,
                    BirthLocationCityList = GetCities(),
                    BirthLocationCityId = item.Birth.Location.CityId,
                    CompanyId = item.Post.CompanyId,
                    CompanyName = item.Post.Company.Name,
                    PostId = item.PostId,
                    PostName = item.Post.Name,
                    DisabilityId = item.DisabilityId,
                    DisabilityList = GetDisabilities(),
                    Email = item.Email,
                    HomePhone = item.HomePhone,
                    MobilePhone = item.MobilePhone,
                    IsPensioner = item.IsPensioner,
                    MaritalStatusList = GetMaritalStatuses(),
                    NationalityId = item.NationalityId,
                    NationalityList = GetNationalities(),
                    Revenue = item.Revenue.ToString(),
                    ActualLocationBuildingNumber = actualLocation.Location.BuildingNumber,
                    ActualLocationCityList = GetCities(),
                    ActualLocationCityId = actualLocation.Location.CityId,
                    ActualLocationId = actualLocation.LocationId,
                    ActualLocationStreet = actualLocation.Location.Street,
                    RegistationLocationBuildingNumber = residenceLocation.Location.BuildingNumber,
                    RegistrationLocationCityList = GetCities(),
                    RegistrationLocationCityId = residenceLocation.Location.CityId,
                    RegistrationLocationId = residenceLocation.LocationId,
                    RegistrationLocationStreet = residenceLocation.Location.Street,
                    ActualLocationCityName = actualLocation.Location.City.Name,
                    BirthLocationCityName = item.Birth.Location.City.Name,
                    DisabilityName = item.Disability.Name,
                    MaritalStatusName = GetMaritalStatus(item.MaritalStatus).Name,
                    NationalityName = item.Nationality.Name,
                    RegistrationLocationCityName = residenceLocation.Location.City.Name,
                    MaritalStatusId = GetMaritalStatus(item.MaritalStatus).Id,
                    PassportId = item.Passport.Id,
                });
            }
            return result;
        }

        private List<PersonMinViewModel> ConvertPeopleToPersonMinViewModels(List<Person> people)
        {
            List<PersonMinViewModel> result = new List<PersonMinViewModel>();
            foreach (var item in people)
            {
                result.Add(new PersonMinViewModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    MiddleName = item.MiddleName,
                    PassportId = item.Passport.Id,
                    PassportSeries = item.Passport.Series,
                    PassportNumber = item.Passport.Number,
                });
            }
            return result;
        }

        private bool CheckPassportSeriesAndNumber(string Series, string Number, int? PersonId)
        {
            var passports = GetPassports();

            // if new passport : check if unique 
            // if editing passport : check if unique but self
            if (PersonId != null)
            {
                passports = passports.Where(i => i.PersonId != PersonId).ToList();
            }
            if (passports.Any(i => i.Number == Number && i.Series == Series))
            {
                return false;
            }
            return true;
        }

        private bool CheckPassportIdentifyingNumber(string IdentifyingNumber, int? PersonId)
        {
            var passports = GetPassports();

            // if new passport : check if unique 
            // if editing passport : check if unique but self
            if (PersonId != null)
            {
                passports = passports.Where(i => i.PersonId != PersonId).ToList();
            }
            if (passports.Any(i => i.IdentifyingNumber == IdentifyingNumber))
            {
                return false;
            }
            return true;
        }

        private Company GetCompany(string companyName, bool getUnknownCompanyIfNullPassed)
        {
            companyName = string.IsNullOrEmpty(companyName) ? null : companyName;

            if (companyName == null && !getUnknownCompanyIfNullPassed)
            {
                throw new ArgumentNullException();
            }

            var company = _db.Companies.FirstOrDefault(i => i.Name == companyName);
            if (company == null)
            {
                company = new Company { Name = companyName ?? "Unknown company", };
                _db.Companies.Add(company);
                _db.SaveChanges();
            }
            return company;
        }

        private Post GetPost(string postName, Company company, bool getUnknownPostIfNullPassed, bool getUnknownCompanyIfNullPassed)
        {
            postName = string.IsNullOrEmpty(postName) ? null : postName;
            if (postName == null && !getUnknownPostIfNullPassed)
            {
                throw new ArgumentNullException();
            }

            if (company == null && getUnknownCompanyIfNullPassed)
            {
                company = GetCompany(null, getUnknownCompanyIfNullPassed);
            }

            var post = _db.Posts.FirstOrDefault(i => i.Company == company && i.Name == postName);
            if (post == null)
            {
                post = new Post { Company = company, Name = postName ?? "Unknown post", };
                _db.Posts.Add(post);
                _db.SaveChanges();
            }
            return post;
        }

        private IssuingAuthority CreateIssuingAuthority(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }
            var auth = new IssuingAuthority { Name = name, };
            _db.IssuingAuthorities.Add(auth);
            _db.SaveChanges();
            return auth;
        }

        private Location CreateLocation(City city, string street, string building)
        {
            if (city == null)
            {
                throw new ArgumentNullException();
            }
            var loc = new Location { City = city, Street = street, BuildingNumber = building, };
            _db.Locations.Add(loc);
            _db.SaveChanges();
            return loc;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View(ConvertPeopleToPersonMinViewModels(RetreivePeople()));
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            var person = ConvertPeopleToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id);
            if (person == null)
            {
                return RedirectToAction(nameof(StatusDoesNotExist));
            }

            return View(person);
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            var person = ConvertPeopleToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id);
            if (person == null)
            {
                return RedirectToAction(nameof(StatusDoesNotExist));
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var requiredOne = _db.People.FirstOrDefault(i => i.Id == id);
                _db.People.Remove(requiredOne);
                _db.SaveChanges();
                return RedirectToAction(nameof(StatusSuccess));
            }
            catch
            {
                return RedirectToAction(nameof(StatusFailed));
            }
        }

        public ActionResult StatusSuccess()
        {
            return View();
        }

        public ActionResult StatusFailed()
        {
            return View();
        }

        public ActionResult StatusDoesNotExist()
        {
            return View();
        }

        private PersonFullViewModel RestoreSelectLists(PersonFullViewModel model)
        {
            model.BirthLocationCityList = GetCities();
            model.DisabilityList = GetDisabilities();
            model.MaritalStatusList = GetMaritalStatuses();
            model.NationalityList = GetNationalities();
            model.ActualLocationCityList = GetCities();
            model.RegistrationLocationCityList = GetCities();
            return model;
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View(RestoreSelectLists(new PersonFullViewModel()));
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonFullViewModel model)
        {
            try
            {
                if (!CheckPassportSeriesAndNumber(model.PassportSeries, model.PassportNumber, model.Id))
                {
                    ModelState.TryAddModelError("Passport number and series", "Pair of passport series and number already exists");
                }
                if (!CheckPassportIdentifyingNumber(model.PassportIdentifyingNumber, model.Id))
                {
                    ModelState.TryAddModelError("Passport identifying number", "Passport identifying number already exists");
                }
                if (ModelState.IsValid)
                {
                    Company company = model.CompanyName == null 
                        ? null 
                        : GetCompany(model.CompanyName, false);

                    Post post = model.PostName == null && model.CompanyName == null
                        ? null 
                        : GetPost(model.PostName, company, false, false);

                    Person person = new Person
                    {
                        Revenue = decimal.TryParse(model.Revenue, out _) ? decimal.Parse(model.Revenue) : new decimal?(),
                        Email = string.IsNullOrEmpty(model.Email) ? null : model.Email,
                        HomePhone = string.IsNullOrEmpty(model.HomePhone) ? null : model.HomePhone,
                        MobilePhone = string.IsNullOrEmpty(model.MobilePhone) ? null : model.MobilePhone,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        IsPensioner = model.IsPensioner,
                        Disability = GetDisabilities().FirstOrDefault(i => i.Id == model.DisabilityId),
                        MaritalStatus = GetMaritalStatuses().FirstOrDefault(i => i.Id == model.MaritalStatusId)?.BoolValue ?? false,
                        Nationality = GetNationalities().FirstOrDefault(i => i.Id == model.NationalityId),
                        Post = post,
                    };
                    _db.SaveChanges();

                    Passport passport = new Passport
                    {
                        IdentifyingNumber = model.PassportIdentifyingNumber,
                        IssuingAuthority = _db.IssuingAuthorities.FirstOrDefault(i => model.PassportIssuingAuthorityName == i.Name) 
                            ?? CreateIssuingAuthority(model.PassportIssuingAuthorityName),
                        IssuingDate = model.PassportIssuingDate ?? new DateTime(),
                        Number = model.PassportNumber,
                        Person = person,
                        Series = model.PassportSeries,
                    };
                    _db.SaveChanges();

                    var city = GetCities().FirstOrDefault(i => i.Id == model.BirthLocationCityId);

                    Birth birth = new Birth
                    {
                        Date = model.BirthDate,
                        Location = _db.Locations.FirstOrDefault(j => j.City == city),
                        Person = person,
                    };
                    _db.SaveChanges();

                    Location registrationLcoation = _db.Locations.FirstOrDefault(i => i.BuildingNumber == model.RegistationLocationBuildingNumber
                        && i.Street == model.RegistrationLocationStreet
                        && i.CityId == model.RegistrationLocationCityId)
                        ?? CreateLocation(
                            GetCities().FirstOrDefault(i => i.Id == model.RegistrationLocationCityId),
                            model.RegistrationLocationStreet,
                            model.RegistationLocationBuildingNumber);

                    Location actualLocation = _db.Locations.FirstOrDefault(i => i.BuildingNumber == model.ActualLocationBuildingNumber
                        && i.Street == model.ActualLocationStreet
                        && i.CityId == model.ActualLocationCityId) 
                        ?? CreateLocation(
                            GetCities().FirstOrDefault(i => i.Id == model.ActualLocationCityId),
                            model.ActualLocationStreet,
                            model.ActualLocationBuildingNumber);

                    _db.PersonToLocations.AddRange(
                        new [] 
                        { 
                            new PersonToLocation { IsActual = true, Person = person, Location = actualLocation } ,
                            new PersonToLocation { IsActual = false, Person = person, Location = registrationLcoation } ,
                        });
                    _db.SaveChanges();

                    return RedirectToAction(nameof(StatusSuccess));
                }
                else
                {
                    return View(RestoreSelectLists(model));
                }
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(StatusFailed));
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_db.People.FirstOrDefault(i => i.Id == id));
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Person person)
        {
            try
            {
                _db.Update(person);
                _db.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}