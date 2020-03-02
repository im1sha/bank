using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Controllers
{
    public class AtmController : Controller
    {
        private readonly ILogger<AtmController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly CreditDbEntityRetriever _creditDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;
        private readonly TimeService _timeService;
        private readonly FlowService _flowService;

        public AtmController(BankAppDbContext context, ILogger<AtmController> logger, LinkGenerator linkGenerator,
            TimeService timeService, FlowService flowService)
        {
            _db = context;
            _creditDb = new CreditDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
            _logger = logger;
            _linkGenerator = linkGenerator;
            _timeService = timeService;
            _flowService = flowService;
        }

        private static readonly Dictionary<int, int> _wrongPins = new Dictionary<int, int>();
        private static int? _currentAccountId;
    
        private void CheckAccountAndPin(ModelStateDictionary ModelState, int accountIdOut, string pinOut)
        {
            var acc = _db.Accounts.AsNoTracking().FirstOrDefault(i => i.Id == accountIdOut);

            var accId = acc?.Id ?? -1;
            if (acc == null)
            {
                ModelState.TryAddModelError("Account check", "Account is not found.");
            }
            else
            {
                if (_wrongPins.ContainsKey(accId) && _wrongPins[accId] > 2)
                {
                    ModelState.TryAddModelError("Pin code failure", "Your card is lock because you enter wrong pin code 3 times.");
                }
                else
                {
                    if (pinOut != "1234")
                    {
                        if (_wrongPins.ContainsKey(accId))
                        {
                            _wrongPins[accId] += 1;
                        }
                        else
                        {
                            _wrongPins.Add(accId, 1);
                        }

                        if (_wrongPins[accId] > 2)
                        {
                            ModelState.TryAddModelError("Pin code failure", "Your card is lock because you enter wrong pin code 3 times.");
                        }
                        else
                        {
                            ModelState.TryAddModelError("Pin code check", "Please enter correct pin.");
                        }
                    }
                    else
                    {
                        _wrongPins[accId] = 0;
                    }
                }
            }
        }

        private List<Account> GetActualAccounts()
        {
            return _creditDb.GetAccounts().Where(i =>
                _timeService.CheckTerminationDate(i.TerminationDate) && i.StandardAccount != null && i.StandardAccount.Person != null)
                .ToList();
        }

        public ActionResult Login()
        {
            _currentAccountId = null;

            return View(
                new AtmLoginViewModel
                {
                    AccountList = GetActualAccounts(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AtmLoginViewModel input)
        {
            CheckAccountAndPin(ModelState, input.AccountId, input.PinCode);

            if (ModelState.IsValid)
            {
                _currentAccountId = input.AccountId;
                return RedirectToAction(nameof(ListOfActions));
            }
            else
            {
                return View(
                    new AtmLoginViewModel
                    {
                        AccountList = GetActualAccounts(),
                    });
            }
        }

        public ActionResult ListOfActions()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public ActionResult Confirm(string action)
        {
            return Content("OK");

            //switch (action)
            //{
            //    case nameof(Pay):
            //        RedirectToAction(nameof(Login));
            //        break;
            //    case nameof(Pay):
            //        RedirectToAction(nameof(Login));
            //        break;
            //    case nameof(Pay):
            //        RedirectToAction(nameof(Login));
            //        break;
            //    case nameof(Logout):
            //        RedirectToAction(nameof(Login));
            //        break;
            //    default:
            //        RedirectToAction(nameof(Login));
            //        break;
            //}
        }

        public ActionResult Status()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            throw new NotImplementedException();
        }

        public ActionResult Withdraw()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(new AtmDecimalInputViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(AtmDecimalInputViewModel input)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            //CheckAccountAndPin(ModelState, _currentAccountId, input.PinCode);

            //check amount here

            if (ModelState.IsValid)
            {



                return Content("OK");
                //return View("Info", "");
            }
            else
            {
                return View(
                    new AtmLoginViewModel
                    {
                        AccountList = GetActualAccounts(),
                    });
            }
        }

        public ActionResult Pay()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(AtmDecimalInputViewModel input)
        {
            throw new NotImplementedException();
        }

        public ActionResult Logout()
        {
            return RedirectToAction(nameof(Login));
        }
    }
}

