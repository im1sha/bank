using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                    PassportId = item.PassportId,
                    PassportSeries = item.Passport.Series,
                    PassportNumber = item.Passport.Number,
                    PassportIdentifyingNumber = item.Passport.IdentifyingNumber,
                    PassportIssuingAuthorityId = item.Passport.IssuingAuthorityId,
                    PassportIssuingAuthorityName = item.Passport.IssuingAuthority.Name,
                    PassportIssuingDate = item.Passport.IssuingDate,
                    BirthDate = item.Birth.Date,
                    BirthId = item.BirthId,
                    BirthLocationId = item.Birth.LocationId,
                    BirthLocationCityName = item.Birth.Location.City.Name,
                    BirthLocationCityId = item.Birth.Location.CityId,
                    CompanyId = item.Post.CompanyId,
                    CompanyName = item.Post.Company.Name,
                    PostId = item.PostId,
                    PostName = item.Post.Name,
                    DisabilityId = item.DisabilityId,
                    DisabilityName = item.Disability.Name,
                    Email = item.Email,
                    HomePhone = item.HomePhone,
                    MobilePhone = item.MobilePhone,
                    IsPensioner = item.IsPensioner,
                    MaritalStatus = item.MaritalStatus,
                    NationalityId = item.NationalityId,
                    NationalityName = item.Nationality.Name,
                    Revenue = item.Revenue,
                    ActualLocationBuildingNumber = actualLocation.Location.BuildingNumber,
                    ActualLocationCity = actualLocation.Location.City.Name,
                    ActualLocationCityId = actualLocation.Location.CityId,
                    ActualLocationId = actualLocation.LocationId,
                    ActualLocationStreet = actualLocation.Location.Street,
                    ResidenceLocationBuildingNumber = residenceLocation.Location.BuildingNumber,
                    ResidenceLocationCity = residenceLocation.Location.City.Name,
                    ResidenceLocationCityId =residenceLocation.Location.CityId,
                    ResidenceLocationId = residenceLocation.LocationId,
                    ResidenceLocationStreet = residenceLocation.Location.Street,
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

        // GET: Person/Create
        public ActionResult Create()
        {
            return View(new Person());
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(RetreivePeople());
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

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_db.People.Find(id));
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var movie = _db.People.Find(id);
                _db.People.Remove(movie);
                _db.SaveChanges();
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