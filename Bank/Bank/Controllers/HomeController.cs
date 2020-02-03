using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestBankAppContext _db;

        public HomeController(TestBankAppContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_db.People
                .Include(u => u.Birth).ThenInclude(u => u.Location).ThenInclude(u => u.City)           
                .Include(u => u.Disability)
                .Include(u => u.Nationality)
                .Include(u => u.Passport).ThenInclude(u => u.IssuingAuthority)
                .Include(u => u.PersonToLocations).ThenInclude(u => u.Location).ThenInclude(u => u.City)
                .Include(u => u.Post).ThenInclude(u => u.Company)
                .ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
