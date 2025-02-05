﻿using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bank.Controllers
{
    public class DepositController : Controller
    {
        private readonly ILogger<DepositController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;
        private readonly TimeService _timeService;
        private readonly FlowService _flowService;

        public DepositController(BankAppDbContext context, ILogger<DepositController> logger, LinkGenerator linkGenerator,
            TimeService timeService, FlowService flowService/*, DepositDbEntityRetriever depositDb, PersonDbEntityRetriever personDb*/)
        {
            _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            //_depositDb = depositDb;
            //_personDb = personDb;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _timeService = timeService;
            _flowService = flowService;
        }


        // here month is 30 days
        public ActionResult Skip([FromQuery]bool skipDay, [FromQuery]bool skipMonth)
        {
            if (skipDay)
            {
                SkipDay();
            }
            else if (skipMonth)
            {
                for (int i = 0; i < 30; i++)
                {
                    SkipDay();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private void SkipDay()
        {
            _flowService.SkipDay();
        }

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
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(i.Account.OpenDate, i.Account.TerminationDate)),
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
            var dep = _depositDb.GetDepositAccounts().First(j => j.Id == id);

            var model = new DepositIndexViewModel
            {
                AccountId = dep.Account.Id,
                AccountName = dep.Account.Name,
                AccountNumber = dep.Account.Number,
                Currency = dep.DepositCore.DepositVariable.Currency.Name,
                DepositName = dep.DepositCore.DepositVariable.DepositGeneral.Name,
                Id = dep.Id,
                InterestRate = dep.DepositCore.InterestRate,
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(dep.Account.OpenDate, dep.Account.TerminationDate)),
                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(dep.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
                MoneyAmount = dep.Account.Money.Amount,
                OpenDate = dep.Account.OpenDate,
                Owner = dep.Person.FirstName + " " + dep.Person.LastName,
                OwnerId = dep.Person.Id,
                Passport = dep.Person.Passport.Series + dep.Person.Passport.Number,
                Profit = dep.Profit?.Amount ?? 0m,
                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(dep.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
                Term = dep.DepositCore.InterestAccrual.Name,
                TerminationDate = dep.Account.TerminationDate,
                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(dep.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
            };

            return View(model);
        }

        #region api of interaction with form of deposit creation

        [HttpPost]
        public ActionResult AccountChanged(DepositCreateViewModel model)
        {
            var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).AccountChanged(model);

            return Json(result.MoneyAmount);
        }

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

        // GET: Deposit/Create?personId=5&currencyId=1
        public ActionResult Create(
            [FromQuery]int? personId,
            [FromQuery]int? currencyId = null,
            [FromQuery]int? depositGeneralId = null,
            // [FromQuery]int? accountId = null,
            // [FromQuery]int? interestAccrualId = null,
            [FromQuery]DateTime? openDate = null)
        {
            if (personId == null)
            {
                return View("StatusNotFound");
            }
            try
            {
                var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService)
                    .GenerateNew((int)personId, currencyId, depositGeneralId, null, null, /*accountId, interestAccrualId,*/ openDate);
                return View(result);
            }
            catch (DepositCreateException e)
            {
                switch (e.Reason)
                {
                    case DepositCreateExceptionType.StandardAccountsNotExist:
                        return View(
                            "StatusStandardAccountsNotFound",
                            _linkGenerator.GetUriByAction(
                                HttpContext,
                                nameof(StandardAccountController.Create),
                                "StandardAccount",
                                new { id = personId, isPerson = true }));
                    case DepositCreateExceptionType.PersonNotExist:
                    case DepositCreateExceptionType.AccountsOfGivenCurrencyNotExist:
                    case DepositCreateExceptionType.DepositNotExist:
                    case DepositCreateExceptionType.InterestAccrualNotFound:
                    default:
                        return View("StatusNotFound");
                }
            }
        }

        // POST: Deposit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepositCreateViewModel input)
        {
            var personId = input.OwnerId;
            var currencyId = input.CurrencyId;
            var depositGeneralId = input.DepositGeneralId;
            var accountSourceId = input.AccountSourceId;
            var interestAccrualId = input.InterestAccrualId;
            var openDate = input.OpenDate;
            var selectedMoney = input.SelectedMoney;
            var accName = input.Name;

            //
            // add money transfer check later
            //
            //
            // bool moneyTransfer = false;

            try
            {
                if (!CheckMoneyAmount(currencyId, depositGeneralId, interestAccrualId, accountSourceId, selectedMoney))
                {
                    ModelState.TryAddModelError("Money is out of bounds", "You should enter amount of money that exceeds " +
                        "required amount of money and less than amount of money on your account.");
                }
                if (!CheckOpenDate(openDate))
                {
                    ModelState.TryAddModelError("Open date in the past", "Deposit open date should be not less than current date.");
                }

                // use on else condition
                var vm = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService)
                    .GenerateNew(personId, currencyId, depositGeneralId, accountSourceId, interestAccrualId, openDate);

                if (ModelState.IsValid)
                {
                    // use input data here, not vm

                    var sourceAccount = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);

                    var profit = new Money
                    {
                        Currency = _depositDb.GetCurrencies().First(i => i.Id == currencyId),
                        Amount = 0,
                    };
                    _db.Moneys.Add(profit);
                    _db.SaveChanges();

                    var money = new Money
                    {
                        Currency = _depositDb.GetCurrencies().First(i => i.Id == currencyId),
                        Amount = selectedMoney,
                    };
                    _db.Moneys.Add(money);
                    _db.SaveChanges();

                    var deposit = new DepositAccount
                    {
                        Person = _personDb.GetPeople().First(i => i.Id == personId),
                        DepositCore = _depositDb.GetDepositCores().First(i => i.DepositVariable.DepositGeneralId == depositGeneralId
                            && i.DepositVariable.CurrencyId == currencyId && i.InterestAccrualId == interestAccrualId),
                        Profit = profit,
                    };
                    _db.DepositAccounts.Add(deposit);
                    _db.SaveChanges();

                    var accForDeposit = new Account
                    {
                        DepositAccount = deposit,
                        Name = accName,
                        Number = DbRetrieverUtils.GenerateNewDepositId(_depositDb),
                        OpenDate = openDate,
                        TerminationDate = null,
                        Money = money,
                    };
                    _db.Accounts.Add(accForDeposit);
                    _db.SaveChanges();



                    var source = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);
                    source.Money.Amount -= selectedMoney;
                    _db.Accounts.Update(source);
                    _db.SaveChanges();

                    var bank = _depositDb.GetStandardAccounts().First(i => i.LegalEntity == _depositDb.GetLegalEntities().First(j => j.Name.Contains("BelAPB.by"))
                        && i.Account.Money.CurrencyId == currencyId);
                    bank.Account.Money.Amount += selectedMoney;
                    _db.StandardAccounts.Update(bank);
                    _db.SaveChanges();

                    return View("StatusSucceeded", "Deposit creation succeeded.");
                }
                else
                {
                    return View(vm);
                }
            }
            catch
            {
                return View("StatusFailed", "Deposit creation failed.");
            }
        }

        private bool CheckOpenDate(DateTime openDate)
        {
            return openDate >= _timeService.CurrentTime;
        }

        private bool CheckMoneyAmount(int currencyId, int depositGeneralId, int interestAccrualId, int accountSourceId, decimal moneyAmount)
        {
            try
            {
                var account = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);

                if (account.Money.CurrencyId != currencyId)
                {
                    return false;
                }
                if (account.Money.Amount < moneyAmount)
                {
                    return false;
                }

                return _depositDb.GetDepositVariables()
                    .First(i => i.DepositGeneralId == depositGeneralId
                        && i.DepositCores.Any(i => i.InterestAccrualId == interestAccrualId)
                        && i.CurrencyId == currencyId).MinimalDeposit.Amount <= moneyAmount;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //// GET: Deposit/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Deposit/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Deposit/Delete/5
        // id == depositaccount id
        public ActionResult Delete(int id)
        {
            try
            {
                var depositAccount = _depositDb.GetDepositAccounts().Where(i => i.Id == id).FirstOrDefault();

                if (depositAccount == null)
                {
                    return View("StatusNotFound");
                }
                if (!_timeService.CheckTerminationDate(depositAccount.Account.TerminationDate))
                {
                    return View("StatusFailed", "Account is closed.");
                }
                var model = new DepositIndexViewModel
                {
                    AccountId = depositAccount.Account.Id,
                    AccountName = depositAccount.Account.Name,
                    AccountNumber = depositAccount.Account.Number,
                    Currency = depositAccount.DepositCore.DepositVariable.Currency.Name,
                    DepositName = depositAccount.DepositCore.DepositVariable.DepositGeneral.Name,
                    Id = depositAccount.Id,
                    InterestRate = depositAccount.DepositCore.InterestRate,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(depositAccount.Account.OpenDate, depositAccount.Account.TerminationDate)),
                    IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
                    MoneyAmount = depositAccount.Account.Money.Amount,
                    OpenDate = depositAccount.Account.OpenDate,
                    Owner = depositAccount.Person.FirstName + " " + depositAccount.Person.LastName,
                    OwnerId = depositAccount.Person.Id,
                    Passport = depositAccount.Person.Passport.Series + depositAccount.Person.Passport.Number,
                    Profit = depositAccount.Profit?.Amount ?? 0m,
                    ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
                    Term = depositAccount.DepositCore.InterestAccrual.Name,
                    TerminationDate = depositAccount.Account.TerminationDate,
                    WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
                };
                return View(model);
            }
            catch (Exception)
            {
                return View("StatusFailed", "Deposit delete failed.");
            }
        }

        // POST: Deposit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DepositIndexViewModel model)
        {
            try
            {
                // shouldn't track 'cause _flowService will operate deposit account instance
                var dep = _db.DepositAccounts.Include(i => i.Account)
                    .AsNoTracking()
                    .First(i => i.Id == model.Id);

                if (!_timeService.CheckTerminationDate(dep.Account.TerminationDate))
                {
                    return View("StatusFailed", "Account is closed.");
                }

                _flowService.Close<DepositFlowHandler>(dep.Account.Id, false);

                return View("StatusSucceeded", "Deposit close succeeded.");
            }
            catch
            {
                return View("StatusFailed", "Deposit delete failed.");
            }
        }
    }
}

