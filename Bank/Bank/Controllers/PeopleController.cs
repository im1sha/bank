using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly BankAppContext _db;

        public PeopleController(BankAppContext context, ILogger<PeopleController> logger)
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

        // GET: People
        public ActionResult Index()
        {
            return View(RetreivePeople());
        }

        // GET: People/Details/5
        public ActionResult Details(int id)
        {
            return View(RetreivePeople().FirstOrDefault(i => i.Id == id));
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View(new Person());
        }

        // POST: People/Create
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

        // GET: People/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_db.People.FirstOrDefault(i => i.Id == id));
        }

        // POST: People/Edit/5
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

        // GET: People/Delete/5
        public ActionResult Delete(int id)
        {            
            return View(_db.People.Find(id));
        }

        // POST: People/Delete/5
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