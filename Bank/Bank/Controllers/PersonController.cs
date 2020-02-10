using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
                    CompanyId = item.Post?.CompanyId,
                    CompanyName = item.Post?.Company?.Name,
                    PostId = item.PostId,
                    PostName = item.Post?.Name,
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
                return NotFound();// RedirectToAction(nameof(StatusDoesNotExist));
            }

            return View(person);
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            var person = ConvertPeopleToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id);
            if (person == null)
            {
                return NotFound();// RedirectToAction(nameof(StatusDoesNotExist));
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
                return RedirectToAction(nameof(StatusSuccess), new RouteValueDictionary(new
                {
                    controller = "Person",
                    action = nameof(StatusSuccess),
                    status = "Item was created."
                }));
            }
            catch
            {
                return RedirectToAction(nameof(StatusFailed));
            }
        }

        public ActionResult StatusSuccess(string status)
        {
            return View(status);
        }

        public ActionResult StatusFailed()
        {
            return View();
        }

        //public ActionResult StatusDoesNotExist()
        //{
        //    return View();
        //}

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

        private enum AnalyzeResult { ReturnFormBack, Failed, Succeed };

        private (PersonFullViewModel Model, AnalyzeResult Result) Change(PersonFullViewModel model, bool isCreate)
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

                    Person person = isCreate ? new Person() : RetreivePeople().First(i => i.Id == model.Id);

                    person.Revenue = decimal.TryParse(model.Revenue, out _) ? decimal.Parse(model.Revenue) : new decimal?();
                    person.Email = string.IsNullOrEmpty(model.Email) ? null : model.Email;
                    person.HomePhone = string.IsNullOrEmpty(model.HomePhone) ? null : model.HomePhone;
                    person.MobilePhone = string.IsNullOrEmpty(model.MobilePhone) ? null : model.MobilePhone;
                    person.FirstName = model.FirstName;
                    person.LastName = model.LastName;
                    person.MiddleName = model.MiddleName;
                    person.IsPensioner = model.IsPensioner;
                    person.Disability = GetDisabilities().FirstOrDefault(i => i.Id == model.DisabilityId);
                    person.MaritalStatus = GetMaritalStatuses().FirstOrDefault(i => i.Id == model.MaritalStatusId)?.BoolValue ?? false;
                    person.Nationality = GetNationalities().FirstOrDefault(i => i.Id == model.NationalityId);
                    person.Post = post;

                    if (isCreate)
                    {
                        _db.People.Add(person);
                    }
                    else
                    {
                        _db.Update(person);
                    }
                    _db.SaveChanges();

                    Passport passport = isCreate ? new Passport() : _db.Passports.First(i => i.Person == person);

                    passport.IdentifyingNumber = model.PassportIdentifyingNumber;
                    passport.IssuingAuthority = _db.IssuingAuthorities.FirstOrDefault(i => model.PassportIssuingAuthorityName == i.Name)
                        ?? CreateIssuingAuthority(model.PassportIssuingAuthorityName);
                    passport.IssuingDate = model.PassportIssuingDate ?? new DateTime();
                    passport.Number = model.PassportNumber;
                    passport.Person = person;
                    passport.Series = model.PassportSeries;

                    if (isCreate)
                    {
                        _db.Passports.Add(passport);
                    }
                    else
                    {
                        _db.Update(passport);
                    }
                    _db.SaveChanges();

                    var city = GetCities().FirstOrDefault(i => i.Id == model.BirthLocationCityId);

                    Birth birth = isCreate ? new Birth() : _db.Births.First(i => i.Person == person);

                    birth.Date = model.BirthDate;
                    birth.Location = _db.Locations.FirstOrDefault(j => j.City == city);
                    birth.Person = person;

                    if (isCreate)
                    {
                        _db.Births.Add(birth);
                    }
                    else
                    {
                        _db.Update(birth);
                    }
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

                    if (isCreate)
                    {
                        _db.PersonToLocations.AddRange(
                            new[]
                            {
                                new PersonToLocation { IsActual = true, Person = person, Location = actualLocation } ,
                                new PersonToLocation { IsActual = false, Person = person, Location = registrationLcoation } ,
                            });
                    }
                    else
                    {
                        _db.PersonToLocations.First(i => i.IsActual && i.Person == person).Location = actualLocation;
                        _db.PersonToLocations.First(i => !i.IsActual && i.Person == person).Location = registrationLcoation;
                        _db.PersonToLocations.UpdateRange(_db.PersonToLocations.Where(i => i.Person == person).ToArray());
                    }

                    _db.SaveChanges();

                    return (RestoreSelectLists(model), AnalyzeResult.Succeed);
                }
                else
                {
                    return (RestoreSelectLists(model), AnalyzeResult.ReturnFormBack);
                }
            }
            catch (Exception)
            {
                return (RestoreSelectLists(model), AnalyzeResult.Failed);
            }
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonFullViewModel model)
        {
            var result = Change(model, true);

            switch (result.Result)
            {
                case AnalyzeResult.ReturnFormBack:
                    return View(result.Model);
                case AnalyzeResult.Failed:
                    return RedirectToAction(nameof(StatusFailed));
                case AnalyzeResult.Succeed:
                    return RedirectToAction(nameof(StatusSuccess), new RouteValueDictionary(new { 
                        controller = "Person",
                        action = nameof(StatusSuccess), 
                        status = "Item was created." }));
            }
            throw new ApplicationException();
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            var person = ConvertPeopleToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id);
            if (person == null)
            {
                return NotFound();// RedirectToAction(nameof(StatusDoesNotExist));
            }

            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonFullViewModel model)
        {
            var result = Change(model, false);

            switch (result.Result)
            {
                case AnalyzeResult.ReturnFormBack:
                    return View(result.Model);
                case AnalyzeResult.Failed:
                    return RedirectToAction(nameof(StatusFailed));
                case AnalyzeResult.Succeed:
                    return RedirectToAction(nameof(StatusSuccess), new
                    {
                        //controller = "Person",
                        //action = nameof(StatusSuccess),
                        status = "Item was changed."
                    });
            }
            throw new ApplicationException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

