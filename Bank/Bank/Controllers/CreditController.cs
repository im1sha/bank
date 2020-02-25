using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bank.Controllers
{
    public class CreditController : Controller
    {
    //    private readonly ILogger<PersonController> _logger;
    //    private readonly LinkGenerator _linkGenerator;
    //    private readonly DepositDbEntityRetriever _depositDb;
    //    private readonly PersonDbEntityRetriever _personDb;
    //    private readonly BankAppDbContext _db;
    //    private readonly TimeService _timeService;

    //    public CreditController(BankAppDbContext context, ILogger<PersonController> logger, LinkGenerator linkGenerator, TimeService timeService)
    //    {
    //        _db = context;
    //        _depositDb = new DepositDbEntityRetriever(context);
    //        _personDb = new PersonDbEntityRetriever(context);
    //        _logger = logger;
    //        _linkGenerator = linkGenerator;
    //        _timeService = timeService;
    //    }

    //    // here month is 30 days
    //    public ActionResult Skip([FromQuery]bool skipDay, [FromQuery]bool skipMonth)
    //    {
    //        if (skipDay)
    //        {
    //            SkipDay();
    //        }
    //        else if (skipMonth)
    //        {
    //            for (int i = 0; i < 30; i++)
    //            {
    //                SkipDay();      
    //            }
    //        }

    //        return RedirectToAction(nameof(Index));
    //    }

    //    private void SkipDay()
    //    {
    //        _timeService.AddDays(1);

    //        foreach (var deposit in _depositDb.GetDepositAccounts())
    //        {
    //            if (_timeService.CheckActive(deposit.Account.TerminationDate))
    //            {
    //                // month elapsed (for deposit with capitalization)
    //                if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization
    //                    && _timeService.IsMultipleOfMonth(deposit.Account.OpenDate))
    //                {
    //                    var profit =  (30.0m / 365.0m)
    //                        * (deposit.DepositCore.InterestRate / 100.0m) 
    //                        * (deposit.Account.Money.Amount + deposit.Profit.Amount);
    //                    deposit.Profit.Amount += profit;
    //                    _db.DepositAccounts.Update(deposit);
    //                    _db.SaveChanges();
    //                }
    //                // termination date is now
    //                if (_timeService.CountElapsedDays(deposit.Account.OpenDate) == (deposit.DepositCore.InterestAccrual.TermInDays ?? 365))
    //                {                        
    //                    CloseDeposit(deposit, true);
    //                }
    //            }
    //        }
    //    }

     
    //    private void CloseDeposit(DepositAccount deposit, bool saveProfit)
    //    {
    //        if (saveProfit)
    //        {            
    //            if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
    //            {
    //                var profit = (_timeService.CountElapsedDays(deposit.Account.OpenDate) % 30) / 365.0m
    //                    * (deposit.DepositCore.InterestRate / 100.0m)
    //                    * (deposit.Account.Money.Amount + deposit.Profit.Amount);
    //                deposit.Profit.Amount += profit;
    //            }
    //            else if (!deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
    //            {
    //                var profit = (int)deposit.DepositCore.InterestAccrual.TermInDays / 365.0m
    //                    * (deposit.DepositCore.InterestRate / 100.0m)
    //                    * (deposit.Account.Money.Amount + deposit.Profit.Amount);
    //                deposit.Profit.Amount += profit;
    //            }
    //        }
    //        else
    //        {
    //            if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
    //            {
    //                deposit.Profit.Amount = 0;
    //            }
    //        }

    //        var totalMoney = deposit.Account.Money.Amount + deposit.Profit.Amount;

    //        deposit.Account.TerminationDate = _timeService.CurrentTime;
    //        _db.DepositAccounts.Update(deposit);
    //        _db.SaveChanges();

    //        var bank = _depositDb.GetStandardAccounts().First(i => i.LegalEntity == _depositDb.GetLegalEntities().First()
    //            && i.Account.Money.Currency == deposit.Account.Money.Currency);
    //        bank.Account.Money.Amount -= totalMoney;
    //        _db.StandardAccounts.Update(bank);
    //        _db.SaveChanges();

    //        // create standard account with equal amount of money
    //        var standardAccount = new StandardAccount
    //        {
    //            Person = deposit.Person,
    //        };
    //        _db.StandardAccounts.Add(standardAccount);
    //        _db.SaveChanges();

    //        var acc = new Account
    //        {
    //            Name = $"closed deposit {deposit.Account.Number}",
    //            Number = DbRetrieverUtils.GenerateNewStandardAccountId(_depositDb),
    //            OpenDate = _timeService.CurrentTime,
    //            StandardAccount = standardAccount,
    //        };
    //        _db.Accounts.Add(acc);
    //        _db.SaveChanges();

    //        var money = new Money
    //        {
    //            Currency = deposit.Account.Money.Currency,
    //            Account = acc,
    //            Amount = totalMoney,
    //        };
    //        _db.Moneys.Add(money);
    //        _db.SaveChanges();
    //    }

    //    // GET: Deposit
    //    //      Deposit/index/5
    //    // id == person id
    //    public ActionResult Index(int? id)
    //    {
    //        var accs = _depositDb.GetDepositAccounts().Where(i => id == null ? true : i.PersonId == id).ToList();

    //        var models = accs.Select(i => new DepositIndexViewModel
    //        {
    //            AccountId = i.Account.Id,
    //            AccountName = i.Account.Name,
    //            AccountNumber = i.Account.Number,
    //            Currency = i.DepositCore.DepositVariable.Currency.Name,
    //            DepositName = i.DepositCore.DepositVariable.DepositGeneral.Name,
    //            Id = i.Id,
    //            InterestRate = i.DepositCore.InterestRate,
    //            IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.CheckActive(i.Account.TerminationDate)),
    //            IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
    //            MoneyAmount = i.Account.Money.Amount,
    //            OpenDate = i.Account.OpenDate,
    //            Owner = i.Person.FirstName + " " + i.Person.LastName,
    //            OwnerId = i.Person.Id,
    //            Passport = i.Person.Passport.Series + i.Person.Passport.Number,
    //            Profit = i.Profit?.Amount ?? 0m,
    //            ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
    //            Term = i.DepositCore.InterestAccrual.Name,
    //            TerminationDate = i.Account.TerminationDate,
    //            WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(i.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),

    //        }).ToList();

    //        return View(models);
    //    }

    //    // GET: Deposit/Details/5
    //    public ActionResult Details(int id)
    //    {
    //        var acc = _depositDb.GetDepositAccounts().First(j => j.Id == id);

    //        var model = new DepositIndexViewModel
    //        {
    //            AccountId = acc.Account.Id,
    //            AccountName = acc.Account.Name,
    //            AccountNumber = acc.Account.Number,
    //            Currency = acc.DepositCore.DepositVariable.Currency.Name,
    //            DepositName = acc.DepositCore.DepositVariable.DepositGeneral.Name,
    //            Id = acc.Id,
    //            InterestRate = acc.DepositCore.InterestRate,
    //            IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.CheckActive(acc.Account.TerminationDate)),
    //            IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
    //            MoneyAmount = acc.Account.Money.Amount,
    //            OpenDate = acc.Account.OpenDate,
    //            Owner = acc.Person.FirstName + " " + acc.Person.LastName,
    //            OwnerId = acc.Person.Id,
    //            Passport = acc.Person.Passport.Series + acc.Person.Passport.Number,
    //            Profit = acc.Profit?.Amount ?? 0m,
    //            ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
    //            Term = acc.DepositCore.InterestAccrual.Name,
    //            TerminationDate = acc.Account.TerminationDate,
    //            WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
    //        };

    //        return View(model);
    //    }

    //    #region api of interaction with form of deposit creation

    //    [HttpPost]
    //    public ActionResult AccountChanged(DepositCreateViewModel model)
    //    {
    //        var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).AccountChanged(model);

    //        return Json(result.MoneyAmount);
    //    }

    //    [HttpPost]
    //    public ActionResult DateChanged(DepositCreateViewModel model)
    //    {
    //        return Json(new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).DateChanged(model));
    //    }

    //    [HttpPost]
    //    public ActionResult TermChanged(DepositCreateViewModel model)
    //    {
    //        var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService).TermChanged(model);

    //        return Json(new { openDate = result.OpenDate, terminationDate = result.TerminationDate, interestRate = result.InterestRate, });
    //    }

    //    #endregion

    //    // GET: Deposit/Create?personId=5&currencyId=1
    //    public ActionResult Create(
    //        [FromQuery]int? personId,
    //        [FromQuery]int? currencyId = null,
    //        [FromQuery]int? depositGeneralId = null,
    //        // [FromQuery]int? accountId = null,
    //        // [FromQuery]int? interestAccrualId = null,
    //        [FromQuery]DateTime? openDate = null)
    //    {
    //        if (personId == null)
    //        {
    //            return View("StatusNotFound");
    //        }
    //        try
    //        {
    //            var result = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService)
    //                .GenerateNew((int)personId, currencyId, depositGeneralId, null, null, /*accountId, interestAccrualId,*/ openDate);
    //            return View(result);
    //        }
    //        catch (DepositCreateException e)
    //        {
    //            switch (e.Reason)
    //            {
    //                case DepositCreateExceptionType.StandardAccountsNotExist:
    //                    return View(
    //                        "StatusStandardAccountsNotFound",
    //                        _linkGenerator.GetUriByAction(
    //                            HttpContext,
    //                            nameof(StandardAccountController.Create),
    //                            "StandardAccount",
    //                            new { id = personId, isPerson = true }));
    //                case DepositCreateExceptionType.PersonNotExist:
    //                case DepositCreateExceptionType.AccountsOfGivenCurrencyNotExist:
    //                case DepositCreateExceptionType.DepositNotExist:
    //                case DepositCreateExceptionType.InterestAccrualNotFound:
    //                default:
    //                    return View("StatusNotFound");
    //            }
    //        }
    //    }

    //    // POST: Deposit/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create(DepositCreateViewModel input)
    //    {
    //        var personId = input.OwnerId;
    //        var currencyId = input.CurrencyId;
    //        var depositGeneralId = input.DepositGeneralId;
    //        var accountSourceId = input.AccountSourceId;
    //        var interestAccrualId = input.InterestAccrualId;
    //        var openDate = input.OpenDate;
    //        var selectedMoney = input.SelectedMoney;
    //        var accName = input.Name;

    //        //
    //        // add money transfer check later
    //        //
    //        //
    //        // bool moneyTransfer = false;

    //        try
    //        {
    //            if (!CheckMoneyAmount(currencyId, depositGeneralId, interestAccrualId, accountSourceId, selectedMoney))
    //            {
    //                ModelState.TryAddModelError("Money is out of bounds", "You should enter amount of money that exceeds " +
    //                    "required amount of money and less than amount of money on your account.");
    //            }
    //            if (!CheckOpenDate(openDate))
    //            {
    //                ModelState.TryAddModelError("Open date in the past", "Deposit open date should be not less than current date.");
    //            }

    //            // use on else condition
    //            var vm = new DepositCreateViewModelConstructor(_depositDb, _personDb, _timeService)
    //                .GenerateNew(personId, currencyId, depositGeneralId, accountSourceId, interestAccrualId, openDate);

    //            if (ModelState.IsValid)
    //            {
    //                // use input data here, not vm

    //                var sourceAccount = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);
    //                var deposit = new DepositAccount
    //                {
    //                    Person = _personDb.GetPeople().First(i => i.Id == personId),
    //                    DepositCore = _depositDb.GetDepositCores().First(i => i.DepositVariable.DepositGeneralId == depositGeneralId
    //                        && i.DepositVariable.CurrencyId == currencyId && i.InterestAccrualId == interestAccrualId),
    //                };
    //                _db.DepositAccounts.Add(deposit);
    //                _db.SaveChanges();

    //                var accForDeposit = new Account
    //                {
    //                    DepositAccount = deposit,
    //                    Name = accName,
    //                    Number = DbRetrieverUtils.GenerateNewDepositId(_depositDb),
    //                    OpenDate = openDate,
    //                    TerminationDate = null,
    //                };
    //                _db.Accounts.Add(accForDeposit);
    //                _db.SaveChanges();

    //                var profit = new Money
    //                {
    //                    DepositAccount = deposit,
    //                    Currency = _depositDb.GetCurrencies().First(i => i.Id == currencyId),
    //                    Amount = 0,
    //                };
    //                _db.Moneys.Add(profit);
    //                _db.SaveChanges();

    //                var money = new Money
    //                {
    //                    Account = accForDeposit,
    //                    Currency = _depositDb.GetCurrencies().First(i => i.Id == currencyId),
    //                    Amount = selectedMoney,
    //                };
    //                _db.Moneys.Add(money);
    //                _db.SaveChanges();

    //                var source = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);
    //                source.Money.Amount -= selectedMoney;
    //                _db.Accounts.Update(source);
    //                _db.SaveChanges();

    //                var bank = _depositDb.GetStandardAccounts().First(i => i.LegalEntity == _depositDb.GetLegalEntities().First()
    //                    && i.Account.Money.CurrencyId == currencyId);
    //                bank.Account.Money.Amount += selectedMoney;
    //                _db.StandardAccounts.Update(bank);
    //                _db.SaveChanges();

    //                return View("StatusSucceeded", "Deposit creation succeeded.");
    //            }
    //            else
    //            {
    //                return View(vm);
    //            }
    //        }
    //        catch
    //        {
    //            return View("StatusFailed", "Deposit creation failed.");
    //        }
    //    }

    //    private bool CheckOpenDate(DateTime openDate)
    //    {
    //        return openDate >= _timeService.CurrentTime;
    //    }

    //    private bool CheckMoneyAmount(int currencyId, int depositGeneralId, int interestAccrualId, int accountSourceId, decimal moneyAmount)
    //    {
    //        try
    //        {
    //            var account = _depositDb.GetAccounts().First(i => i.Id == accountSourceId);

    //            if (account.Money.CurrencyId != currencyId)
    //            {
    //                return false;
    //            }
    //            if (account.Money.Amount < moneyAmount)
    //            {
    //                return false;
    //            }

    //            return _depositDb.GetDepositVariables()
    //                .First(i => i.DepositGeneralId == depositGeneralId
    //                    && i.DepositCores.Any(i => i.InterestAccrualId == interestAccrualId)
    //                    && i.CurrencyId == currencyId).MinimalDeposit.Amount <= moneyAmount;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }

    //    //// GET: Deposit/Edit/5
    //    //public ActionResult Edit(int id)
    //    //{
    //    //    return View();
    //    //}

    //    //// POST: Deposit/Edit/5
    //    //[HttpPost]
    //    //[ValidateAntiForgeryToken]
    //    //public ActionResult Edit(int id, IFormCollection collection)
    //    //{
    //    //    try
    //    //    {
    //    //        // TODO: Add update logic here

    //    //        return RedirectToAction(nameof(Index));
    //    //    }
    //    //    catch
    //    //    {
    //    //        return View();
    //    //    }
    //    //}

    //    // GET: Deposit/Delete/5
    //    // id == depositaccount id
    //    public ActionResult Delete(int id)
    //    {
    //        try
    //        {
    //            var depositAccount = _depositDb.GetDepositAccounts().Where(i => i.Id == id).FirstOrDefault();

    //            if (depositAccount == null)
    //            {
    //                return View("StatusNotFound");
    //            }
    //            if (!_timeService.CheckActive(depositAccount.Account.TerminationDate))
    //            {
    //                return View("StatusFailed", "Account is closed.");
    //            }
    //            var model = new DepositIndexViewModel
    //            {
    //                AccountId = depositAccount.Account.Id,
    //                AccountName = depositAccount.Account.Name,
    //                AccountNumber = depositAccount.Account.Number,
    //                Currency = depositAccount.DepositCore.DepositVariable.Currency.Name,
    //                DepositName = depositAccount.DepositCore.DepositVariable.DepositGeneral.Name,
    //                Id = depositAccount.Id,
    //                InterestRate = depositAccount.DepositCore.InterestRate,
    //                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.CheckActive(depositAccount.Account.TerminationDate)),
    //                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.IsRevocable),
    //                MoneyAmount = depositAccount.Account.Money.Amount,
    //                OpenDate = depositAccount.Account.OpenDate,
    //                Owner = depositAccount.Person.FirstName + " " + depositAccount.Person.LastName,
    //                OwnerId = depositAccount.Person.Id,
    //                Passport = depositAccount.Person.Passport.Series + depositAccount.Person.Passport.Number,
    //                Profit = depositAccount.Profit?.Amount ?? 0m,
    //                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.ReplenishmentAllowed),
    //                Term = depositAccount.DepositCore.InterestAccrual.Name,
    //                TerminationDate = depositAccount.Account.TerminationDate,
    //                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(depositAccount.DepositCore.DepositVariable.DepositGeneral.WithCapitalization),
    //            };
    //            return View(model);
    //        }
    //        catch (Exception)
    //        {
    //            return View("StatusFailed", "Deposit delete failed.");
    //        }
    //    }

    //    // POST: Deposit/Delete/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Delete(int id, DepositIndexViewModel model)
    //    {
    //        try
    //        {
    //            var dep = _depositDb.GetDepositAccounts().First(i => i.Id == model.Id);
    //            if (!_timeService.CheckActive(dep.Account.TerminationDate))
    //            {
    //                return View("StatusFailed", "Account is closed.");
    //            }

    //            CloseDeposit(dep, false);

    //            return View("StatusSucceeded", "Deposit close succeeded.");
    //        }
    //        catch
    //        {
    //            return View("StatusFailed", "Deposit delete failed.");
    //        }
    //    }
    }
}

