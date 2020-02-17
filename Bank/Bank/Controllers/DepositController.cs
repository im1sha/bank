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
        //      Deposit/index/5
        public ActionResult Index(int? id)
        {         
            var accs = _depositDb.GetDepositAccounts().Where(i => id == null ? true : i.PersonId == id).ToList();
            
            var models = accs.Select(i => new DepositIndexViewModel
            { 
                AccountId = i.Account.Id,
                AccountName = i.Account.Name,
                AccountNumber = i.Account.Number,
                Currency = i.DepositCore.DepositVariable.Currency.Name,
                DepositName = i.DepositCore.DepositVariable.DepositGeneral.Name,
                Id = i.Id,
                InterestRate = i.DepositCore.InterestRate,
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.Account.TerminationDate == null ||  i.Account.TerminationDate > DateTime.Now),
                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
                MoneyAmount = i.Account.Money.Amount,
                OpenDate = i.Account.OpenDate,
                Owner = i.Person.FirstName + " " + i.Person.LastName,
                OwnerId = i.Person.Id,
                Passport = i.Person.Passport.Series + i.Person.Passport.Number,
                Profit = i.Profit?.Amount ?? 0m,
                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
                Term = i.DepositCore.InterestAccrual.Name,
                TerminationDate = i.Account.TerminationDate,
                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
            }).ToList();

            return View(models);
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

            var vm = new DepositCreateViewModel
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

