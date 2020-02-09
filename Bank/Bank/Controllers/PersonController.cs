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

        private List<PersonMinViewModel> ConvertToPersonMinViewModels(List<Person> people)
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
                new MaritalStatusLocal { Id = 1, Name = "Yes" },
                new MaritalStatusLocal { Id = 2, Name = "No" }
            };

        private List<MaritalStatusLocal> GetMarriegeStatuses()
        {
            return _maritalStatusLocals;
        }

        private List<Disability> GetDisabilities()
        {
            return _db.Disabilities.OrderBy(i => i.Id).ToList();
        }

        private List<PersonFullViewModel> ConvertToPersonFullViewModels(List<Person> people)
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
                    BirthId = item.BirthId,
                    BirthLocationId = item.Birth.LocationId,
                    BirthLocationCityName = GetCities(),
                    BirthLocationCityId = item.Birth.Location.CityId,
                    CompanyId = item.Post.CompanyId,
                    CompanyName = item.Post.Company.Name,
                    PostId = item.PostId,
                    PostName = item.Post.Name,
                    DisabilityId = item.DisabilityId,
                    DisabilityName = GetDisabilities(),
                    Email = item.Email,
                    HomePhone = item.HomePhone,
                    MobilePhone = item.MobilePhone,
                    IsPensioner = item.IsPensioner,
                    MaritalStatus = GetMarriegeStatuses(),
                    NationalityId = item.NationalityId,
                    NationalityName = GetNationalities(),
                    Revenue = item.Revenue.ToString(),
                    ActualLocationBuildingNumber = actualLocation.Location.BuildingNumber,
                    ActualLocationCity = GetCities(),
                    ActualLocationCityId = actualLocation.Location.CityId,
                    ActualLocationId = actualLocation.LocationId,
                    ActualLocationStreet = actualLocation.Location.Street,
                    RegistationLocationBuildingNumber = residenceLocation.Location.BuildingNumber,
                    RegistrationLocationCity = GetCities(),
                    RegistrationLocationCityId = residenceLocation.Location.CityId,
                    RegistrationLocationId = residenceLocation.LocationId,
                    RegistrationLocationStreet = residenceLocation.Location.Street,
                });
            }
            return result;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View(ConvertToPersonMinViewModels(RetreivePeople()));
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            return View(ConvertToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id));
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View(ConvertToPersonFullViewModels(RetreivePeople()).FirstOrDefault(i => i.Id == id));
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

        // GET: Person/Create
        public ActionResult Create()
        {        
            return View(RestoreSelectLists(new PersonFullViewModel()));
        }

        private PersonFullViewModel RestoreSelectLists(PersonFullViewModel model)
        {
            model.BirthLocationCityName = GetCities();
            model.DisabilityName = GetDisabilities();
            model.MaritalStatus = GetMarriegeStatuses();
            model.NationalityName = GetNationalities();
            model.ActualLocationCity = GetCities();
            model.RegistrationLocationCity = GetCities();
            return model;
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
                    return RedirectToAction(nameof(StatusSuccess));
                }
                else
                {
                    return View(RestoreSelectLists(model));
                }
            }
            catch
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