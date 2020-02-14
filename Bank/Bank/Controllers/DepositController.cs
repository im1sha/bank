using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Controllers
{
    public class DepositController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly BankAppDbContext _db;

        public DepositController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            _db = context;
            _logger = logger;
        }

        private string ConvertToYesNoFormat(bool x)
        {
            return x ? "Yes" : "No";
        }

        private List<Currency> GetCurrenciesFromDepositVariables()
        {
            return _db.DepositVariables.Include(i => i.Currency).Select(i => i.Currency).OrderBy(i => i.Id).Distinct().ToList();
        }

        private List<DepositGeneral> GetDepositGenerals()
        {
            return _db.DepositGenerals.Include(i => i.DepositVariables).OrderBy(i => i.Id).ToList();
        }

        [HttpPost]
        public ActionResult GetCurrencyNameByModel(SelectDepositViewModel model)
        {
            return Json(GetCurrenciesFromDepositVariables().FirstOrDefault(i => i.Id == model.CurrencyId)?.Name);
        }


        // GET: Deposit
        public ActionResult Index()
        {
            var accounts = _db.Accounts.Include(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.DepositAccount).ThenInclude(i => i.Person)
                .Include(i => i.DepositAccount).ThenInclude(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .ToList();

            var result = accounts.Where(i => i.DepositAccount != null && i.DepositAccount.Person != null).Select(i =>
                new DepositIndexViewModel
                {
                    Deposit = i.DepositAccount.DepositCore.DepositVariable.DepositGeneral.Name,
                    AccountId = i.Id,
                    AccountNumber = i.Number,
                    IsActive = ConvertToYesNoFormat(i.TerminationDate == null || i.TerminationDate < DateTime.Now),
                    CurrencyName = i.Money.Currency.Name,
                    FirstName = i.DepositAccount.Person.FirstName,
                    LastName = i.DepositAccount.Person.LastName,
                    PersonId = i.DepositAccount.Person.Id,
                });
            return View(result);
        }

        // GET: Deposit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private 

        // GET: Deposit/Create
        public ActionResult Create()
        {                  
            var vm = new SelectDepositViewModel
            {
                CurrencyId = ,
                CurrencyList = GetCurrenciesFromDepositVariables(),
                DepositGeneralId = ,
                DepositGeneralList = ,
                InterestAccrualId = ,
                InterestAccrualList =,
                DepositCoreList =,
                DepositCoreId=,
                IsRevocable=,
                WithCapitalization=,
                ReplenishmentAllowed=,
                RequiredMoney=,
                SelectedMoney=,
                StartDate=,
                TotalMoney=,               
            };
            return View("SelectCurrency", vm);            
        }

        // POST: Deposit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: Deposit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Deposit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Deposit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Deposit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}