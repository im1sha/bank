using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Controllers
{
    public class DepositController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;

        private readonly TimeService _timeService = new TimeService();
      
        public DepositController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
        }

        #region api
        [HttpPost]
        public ActionResult CurrencyChanged(DepositCreateViewModel model)
        {
            return Json(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).CurrencyChanged(model));
        }

        [HttpPost]
        public ActionResult AccountChanged(DepositCreateViewModel model)
        {
            var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).AccountChanged(model);

            return Json(result.MoneyAmount);
        }

        [HttpPost]
        public ActionResult DepositChanged(DepositCreateViewModel model)
        {
            return Json(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).DepositChanged(model));
        }

        //[HttpPost]
        //public ActionResult MoneyChanged(DepositCreateViewModel model)
        //{
        //    return Json(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).MoneyChanged(model));
        //}

        [HttpPost]
        public ActionResult DateChanged(DepositCreateViewModel model)
        {
            return Json(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).DateChanged(model));
        }

        [HttpPost]
        public ActionResult TermChanged(DepositCreateViewModel model)
        {
            var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).TermChanged(model);

            return Json(new { openDate = result.OpenDate, terminationDate = result.TerminationDate, interestRate = result.InterestRate, });
        }

        #endregion

        // GET: Deposit
        //      Deposit/index/5
        // id == person id
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
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.CheckActive(i.Account.TerminationDate)),
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
            var acc = _depositDb.GetDepositAccounts().First(j => j.Id == id);

            var model = new DepositIndexViewModel
            {
                AccountId = acc.Account.Id,
                AccountName = acc.Account.Name,
                AccountNumber = acc.Account.Number,
                Currency = acc.DepositCore.DepositVariable.Currency.Name,
                DepositName = acc.DepositCore.DepositVariable.DepositGeneral.Name,
                Id = acc.Id,
                InterestRate = acc.DepositCore.InterestRate,
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.CheckActive(acc.Account.TerminationDate)),
                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
                MoneyAmount = acc.Account.Money.Amount,
                OpenDate = acc.Account.OpenDate,
                Owner = acc.Person.FirstName + " " + acc.Person.LastName,
                OwnerId = acc.Person.Id,
                Passport = acc.Person.Passport.Series + acc.Person.Passport.Number,
                Profit = acc.Profit?.Amount ?? 0m,
                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
                Term = acc.DepositCore.InterestAccrual.Name,
                TerminationDate = acc.Account.TerminationDate,
                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
            };

            return View(model);
        }

        // GET: Deposit/Create/5
        // where 5 is personId
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View("StatusNotFound");
            }

            return View(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).Generate((int)id));
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

