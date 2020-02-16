using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bank.Controllers
{
    public class DepositController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;

        public DepositController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
        }

        //[HttpPost]
        //public ActionResult GetCurrencyNameByModel(SelectDepositViewModel model)
        //{
        //    return Json(string.Empty);
        //}

        // GET: Deposit
        public ActionResult Index()
        {
            var result = _depositDb.GetAccounts().Where(i => i.DepositAccount != null && i.DepositAccount.Person != null)
                .Select(i => new DepositIndexOverallViewModel
                {
                    Passport = i.DepositAccount.Person.Passport.Series + i.DepositAccount.Person.Passport.Number,
                    Deposit = i.DepositAccount.DepositCore.DepositVariable.DepositGeneral.Name,
                    AccountId = i.Id,
                    AccountNumber = i.Number,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.TerminationDate == null || i.TerminationDate > DateTime.Now),
                    CurrencyName =  i.Money.Currency.Name,
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

        // GET: Deposit/Create
        public ActionResult Create()
        {
            var currencyList = _depositDb.GetCurrencies().Where(i => i.DepositVariables.Any()).ToList();//.Take(1).ToList();
            var currencyId = currencyList.First().Id ;
           
            var depositVariableList = _depositDb.GetDepositVariables().Where(i => i.CurrencyId == currencyId).ToList();
            var depositVariableId = depositVariableList.First().Id;
           
            var depositGeneralList = _depositDb.GetDepositGenerals().Where(i => i.DepositVariables.Any(j => depositVariableList.Contains(j))).ToList();
            var depsoitGeneralId = depositGeneralList.First(i => i.DepositVariables.Contains(depositVariableList.First())).Id;
            
            var coreList = _depositDb.GetDepositCores().Where(i => depositVariableList.Contains(i.DepositVariable)).ToList();
            var core = coreList.First(i => i.DepositVariableId == depositVariableId);

            var interestAccrualList = coreList.Select(i => i.InterestAccrual).Distinct().ToList();
            var interestAccrualId = interestAccrualList.First().Id;

            var vm = new CreateDepositViewModel
            {
                CurrencyId = currencyId,
                CurrencyList = currencyList,

                DepositGeneralId = depsoitGeneralId,
                DepositGeneralList = depositGeneralList,

                InterestAccrualId = interestAccrualId,
                InterestAccrualList = interestAccrualList,

                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(core.DepositVariable.DepositGeneral.IsRevocable),
                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(core.DepositVariable.DepositGeneral.WithCapitalization),
                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(core.DepositVariable.DepositGeneral.ReplenishmentAllowed),

                StartDate = DateTime.Now,

                RequiredMoney = core.DepositVariable.MinimalDeposit.Amount,
                SelectedMoney = 0,
                TotalMoney = 999999,
            };
            return View(vm);
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