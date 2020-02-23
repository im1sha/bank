using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bank.Controllers
{
    public class StandardAccountController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;
        private readonly TimeService _timeService;

        public StandardAccountController(BankAppDbContext context, ILogger<PersonController> logger, TimeService timeService)
        {
            _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
            _timeService = timeService;
        }

        // GET: StandardAccount/
        //      StandardAccount/Index/5&isPerson=true
        // id is personId or LegalEntityId
        public ActionResult Index(int? id, bool isPerson = true)
        {
            var accs = _depositDb.GetStandardAccounts()
                .Where(i => id == null ? true : (isPerson ? i.PersonId == id : i.LegalEntityId == id))
                .Where(i => i.Account?.TerminationDate == null || i.Account?.TerminationDate > _timeService.CurrentTime)
                .Select(i => new StandardAccountIndexViewModel
                {
                    Amount = i.Account.Money.Amount,
                    Currency = i.Account.Money.Currency.Name,
                    Owner = i?.LegalEntity?.Name ?? ((i?.Person?.FirstName ?? "") + " " + (i?.Person?.LastName ?? "")),
                    Id = i.Id,
                    Name = i.Account.Name,
                    Number = i.Account.Number,
                    Passport = i?.Person?.Passport?.Series + i?.Person?.Passport?.Number,
                    PersonId = i.PersonId,
                    LegalEntityId = i.LegalEntityId,
                    IsPerson = i.LegalEntityId == null,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.Account?.TerminationDate == null || i.Account?.TerminationDate > _timeService.CurrentTime),
                });
            return View(accs);
        }

        // GET: StandardAccount/Details/5
        public ActionResult Details(int id)
        {
            var acc = _depositDb.GetStandardAccounts().Where(i => i.Id == id)
                .Select(i => new StandardAccountIndexViewModel
                {
                    Amount = i.Account.Money.Amount,
                    Currency = i.Account.Money.Currency.Name,
                    Owner = i?.LegalEntity?.Name ?? ((i?.Person?.FirstName ?? "") + " " + (i?.Person?.LastName ?? "")),
                    Id = i.Id,
                    Name = i.Account.Name,
                    Number = i.Account.Number,
                    Passport = (i?.Person?.Passport?.Series + i?.Person?.Passport?.Number) ?? " ",
                    PersonId = i.PersonId,
                    LegalEntityId = i.LegalEntityId,
                    IsPerson = i.LegalEntityId == null,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.Account?.TerminationDate == null || i.Account?.TerminationDate > _timeService.CurrentTime),
                    OpenDate = i.Account.OpenDate,
                    TerminationDate = i.Account.TerminationDate,
                }).First();
            return View(acc);
        }

        // GET: StandardAccount/Create/5&isPerson=true
        public ActionResult Create(int? id, bool isPerson)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var model = new StandardAccountCreateViewModel
                {
                    Amount = "100",
                    CurrencyId = 1,
                    CurrencyList = _depositDb.GetCurrencies(),
                    IsPerson = isPerson,
                    Name = "acc name",
                    Owner = isPerson
                        ? (_personDb.GetPeople().First(i => i.Id == id).FirstName + " " + _personDb.GetPeople().First(i => i.Id == id).LastName)
                        : _depositDb.GetLegalEntities().First(i => i.Id == id).Name,
                    OwnerId = (int)id,
                    Number = DbRetrieverUtils.GenerateNewStandardAccountId(_depositDb),
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private StandardAccountCreateViewModel RestoreLists(StandardAccountCreateViewModel account)
        {
            account.CurrencyList = _depositDb.GetCurrencies();
            return account;
        }

        // POST: StandardAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StandardAccountCreateViewModel model)
        {
            RestoreLists(model);

            LegalEntity legalEntity = null;
            Person person = null;
            StandardAccount standardAccount = null;
            Account acc = null;
            Money money = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.IsPerson)
                    {
                        person = _personDb.GetPeople().First(i => i.Id == model.OwnerId);
                    }
                    else
                    {
                        legalEntity = _depositDb.GetLegalEntities().First(i => i.Id == model.OwnerId);
                    }
                    standardAccount = new StandardAccount
                    {
                        LegalEntity = legalEntity,
                        Person = person,
                    };

                    _db.StandardAccounts.Add(standardAccount);
                    _db.SaveChanges();

                    acc = new Account
                    {
                        Name = model.Name,
                        Number = DbRetrieverUtils.GenerateNewStandardAccountId(_depositDb),
                        OpenDate = _timeService.CurrentTime,
                        StandardAccount = standardAccount,
                    };

                    _db.Accounts.Add(acc);
                    _db.SaveChanges();

                    money = new Money
                    {
                        Currency = model.CurrencyList[model.CurrencyId - 1],
                        Account = acc,
                        Amount = decimal.Parse(model.Amount),
                    };

                    _db.Moneys.Add(money);
                    _db.SaveChanges();

                    return View("StatusSucceeded", "Standard account create succeeded.");
                }
                return View(model);
            }
            catch
            {
                if (money != null)
                {
                    _db.Moneys.Remove(money);
                    _db.SaveChanges();
                }
                if (acc != null)
                {
                    _db.Accounts.Remove(acc);
                    _db.SaveChanges();
                }
                if (standardAccount != null)
                {
                    _db.StandardAccounts.Remove(standardAccount);
                    _db.SaveChanges();
                }
                return View("StatusFailed", "Standard account create failed.");
            }
        }

        // GET: StandardAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var acc = _depositDb.GetStandardAccounts().Where(i => i.Id == id).First();
                var model = new StandardAccountCreateViewModel
                {
                    Amount = acc.Account.Money.Amount.ToString(),
                    CurrencyName = acc.Account.Money.Currency.Name,
                    Owner = acc?.LegalEntity?.Name ?? ((acc?.Person?.FirstName ?? "") + " " + (acc?.Person?.LastName ?? "")),
                    Id = acc.Id,
                    OwnerId = acc.LegalEntityId == null ? (int)acc.PersonId : (int)acc.LegalEntityId,
                    Name = acc.Account.Name,
                    Number = acc.Account.Number,
                    IsPerson = acc.LegalEntityId == null,
                };

                if (acc.Account.TerminationDate <= _timeService.CurrentTime)
                {
                    return View("StatusFailed", "Account is closed.");
                }
                return View(model);
            }
            catch
            {
                return View("StatusFailed", "Account edit failed.");
            }
        }

        // POST: StandardAccount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StandardAccountCreateViewModel model)
        {
            RestoreLists(model);

            LegalEntity legalEntity = null;
            Person person = null;
            StandardAccount standardAccount = null;
            Account acc = null;
            Money money = null;

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.IsPerson)
                    {
                        person = _personDb.GetPeople().First(i => i.Id == model.OwnerId);
                    }
                    else
                    {
                        legalEntity = _depositDb.GetLegalEntities().First(i => i.Id == model.OwnerId);
                    }
                    standardAccount = _depositDb.GetStandardAccounts().First(i => i.Id == model.Id);

                    acc = _depositDb.GetAccounts().First(i => i.StandardAccount == standardAccount);

                    if (acc.TerminationDate <= _timeService.CurrentTime)
                    {
                        return View("StatusFailed", "Account is closed.");
                    }

                    acc.Name = model.Name;
                    _db.Accounts.Update(acc);
                    _db.SaveChanges();

                    money = _depositDb.GetMoneys().First(i => i.Account == acc);
                    money.Amount = decimal.Parse(model.Amount);
                    _db.Moneys.Update(money);
                    _db.SaveChanges();

                    return View("StatusSucceeded", "Standard account edit succeeded.");
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View("StatusFailed", "Standard account edit failed.");
            }
        }

        // GET: StandardAccount/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var acc = _depositDb.GetStandardAccounts().Where(i => i.Id == id).First();

                if (acc.Account.TerminationDate <= _timeService.CurrentTime)
                {
                    return View("StatusFailed", "Account is closed.");
                }

                var model = new StandardAccountIndexViewModel
                {
                    Amount = acc.Account.Money.Amount,
                    Currency = acc.Account.Money.Currency.Name,
                    Owner = acc?.LegalEntity?.Name ?? ((acc?.Person?.FirstName ?? "") + " " + (acc?.Person?.LastName ?? "")),
                    Id = acc.Id,
                    Name = acc.Account.Name,
                    Number = acc.Account.Number,
                    Passport = (acc?.Person?.Passport?.Series + acc?.Person?.Passport?.Number) ?? " ",
                    PersonId = acc.PersonId,
                    LegalEntityId = acc.LegalEntityId,
                    IsPerson = acc.LegalEntityId == null,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(acc.Account?.TerminationDate == null || acc.Account?.TerminationDate > _timeService.CurrentTime),
                    OpenDate = acc.Account.OpenDate,
                    TerminationDate = acc.Account.TerminationDate,
                };
                return View(model);
            }
            catch (Exception)
            {
                return View("StatusFailed", "Standard account delete failed.");
            }
        }

        // POST: StandardAccount/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, StandardAccountIndexViewModel model)
        {
            try
            {
                var acc = _depositDb.GetAccounts().First(i => i.StandardAccountId == model.Id);
                if (acc.TerminationDate <= _timeService.CurrentTime)
                {
                    return View("StatusFailed", "Account is closed.");
                }
                acc.TerminationDate = _timeService.CurrentTime;
                acc.Money.Amount = 0m;
                _db.Update(acc);             
                _db.SaveChanges();
                return View("StatusSucceeded", "Standard account close succeeded.");               
            }
            catch
            {
                return View("StatusFailed", "Standard account delete failed.");
            }
        }
    }
}


