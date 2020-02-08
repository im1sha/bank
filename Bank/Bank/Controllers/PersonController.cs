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

        private List<PersonShortViewModel> CreatePersonShortViewModels(List<Person> people) 
        {
            List<PersonShortViewModel> result = new List<PersonShortViewModel>();
            foreach (var item in people)
            {
                result.Add(new PersonShortViewModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    MiddleName = item.MiddleName,
                    PassportId = item.Passport.Id,
                    PassportSeries =item.Passport.Series,
                    PassportNumber = item.Passport.Number,
                });
            }
            return result;
        }

        // GET: Person
        public ActionResult Index()
        {
            return View(CreatePersonShortViewModels(RetreivePeople()));
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            return View(RetreivePeople().FirstOrDefault(i => i.Id == id));
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