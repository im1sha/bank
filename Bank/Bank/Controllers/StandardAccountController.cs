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

        public StandardAccountController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
        }

        // GET: StandardAccount/
        //      StandardAccount/Index/5&isPerson=true
        // id is personId or LegalEntityId
        public ActionResult Index(int? id, bool isPerson = true)
        {
            var accs = _depositDb.GetStandardAccounts().Where(i => id == null ? true : (isPerson ? i.PersonId == id : i.LegalEntityId == id))
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
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.Account?.TerminationDate == null || i.Account?.TerminationDate > DateTime.Now),
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
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(i.Account?.TerminationDate == null || i.Account?.TerminationDate > DateTime.Now),
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
                    Number = OutputFormatUtils.GenerateNewStandardAccountId(_depositDb),
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
                        Number = OutputFormatUtils.GenerateNewStandardAccountId(_depositDb),
                        OpenDate = DateTime.Now,
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
                var acc = _depositDb.GetStandardAccounts().Where(i => i.Id == id)
                   .Select(i => new StandardAccountCreateViewModel
                   {
                       Amount = i.Account.Money.Amount.ToString(),
                       CurrencyName = i.Account.Money.Currency.Name,
                       Owner = i?.LegalEntity?.Name ?? ((i?.Person?.FirstName ?? "") + " " + (i?.Person?.LastName ?? "")),
                       Id = i.Id,
                       OwnerId = i.LegalEntityId == null ? (int)i.PersonId : (int)i.LegalEntityId,
                       Name = i.Account.Name,
                       Number = i.Account.Number,
                       IsPerson = i.LegalEntityId == null,
                   }).First();
                return View(acc);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
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
                    acc.Name = model.Name;
                    _db.Accounts.Update(acc);
                    _db.SaveChanges();

                    money = _depositDb.GetMoneys().First(i => i.Account == acc);
                    money.Amount = decimal.Parse(model.Amount);
                    _db.Moneys.Update(money);
                    _db.SaveChanges();

                    return View("StatusSucceeded", "Standard account edit succeeded.");
                }
                return View(model);
            }
            catch
            {
                return View("StatusFailed", "Standard account edit failed.");
            }
        }

        // GET: StandardAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StandardAccount/Delete/5
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
