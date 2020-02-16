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
        // private readonly BankAppDbContext _db;

        public StandardAccountController(BankAppDbContext context, ILogger<PersonController> logger)
        {
            //  _db = context;
            _depositDb = new DepositDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
        }

        // GET: StandardAccount/
        //      StandardAccount/Index/5
        public ActionResult Index(int? id)
        {
            var accs = _depositDb.GetStandardAccounts().Where(i => id == null ? true : i.Id == id)
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
                  TerminationDate = i.Account.TerminationDate ,
              }).First();
            return View(acc);
        }

        // GET: StandardAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StandardAccount/Create
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
                return View();
            }
        }

        // GET: StandardAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StandardAccount/Edit/5
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
