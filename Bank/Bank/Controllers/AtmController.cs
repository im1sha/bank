using Bank.Models;
using Microsoft.AspNetCore.Mvc;
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
        private static int _currentAccountId;

        public ActionResult Login()
        {
            return View(
                new AtmLoginViewModel 
                { 
                    AccountList = _creditDb.GetAccounts().Where(i => _timeService.CheckTerminationDate(i.TerminationDate)).ToList(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AtmLoginViewModel input)
        {
            var acc = _db.Accounts.AsNoTracking().FirstOrDefault(i => i.Id == input.AccountId);
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
                    if (input.PinCode != "1234")
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

            if (ModelState.IsValid)
            {
                _currentAccountId = accId;
                return Content("OK");
            }
            else
            {
                return View(
                    new AtmLoginViewModel
                    {
                        AccountList = _creditDb.GetAccounts().Where(i => _timeService.CheckTerminationDate(i.TerminationDate)).ToList(),
                    });
            }
        }
    }
}

