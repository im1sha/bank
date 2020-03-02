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

        private const string A1_CELLULAR = "a1";
        private const string LIFE_CELLULAR = "life";
        private const string MTC_CELLULAR = "mtc";
        private const string WITHDRAW_ACTION = "withdraw";

        private static readonly Dictionary<int, int> _wrongPins = new Dictionary<int, int>();
        private static int? _currentAccountId;
        private static string _phoneNumber;
        private static string _cellular;
        private static AtmTransactionViewModel _lastTransaction;

        private bool IsActionOfWithdraw(string input) 
        {
            return input?.ToLower() == WITHDRAW_ACTION;
        }

        private bool IsActionOfCellularPayment(string input)
        {
            return input?.ToLower() == nameof(PhoneNumber).ToLower();
        }

        private bool IsCellular(string cellular)
        {
            switch (cellular?.ToLower())
            {
                case A1_CELLULAR:
                case LIFE_CELLULAR:
                case MTC_CELLULAR:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsNumber(string number)
        {
            return number.Length == 9 && long.TryParse(number, out _);
        }

        private bool IsConfirmAction(string id)
        {
            return IsSelectMoneyAction(id) || id?.ToLower() == nameof(AccountStatus).ToLower();
        }

        private bool IsSelectMoneyAction(string id)
        {
            return new[]
            {
                WITHDRAW_ACTION, 
                nameof(PhoneNumber).ToLower() 
            }.Contains(id?.ToLower());
        }

        private void CheckCellularAndNumber(ModelStateDictionary modelState, string cellular, string number)
        {
            if (!IsNumber(number))
            {
                modelState.TryAddModelError("Phone number", "Phone number has invalid format.");
            }
            if (!IsCellular(cellular))
            {
                modelState.TryAddModelError("Cellular", "This cellular is absent.");
            }
        }

        private void CheckAccountAndPin(ModelStateDictionary modelState, int accountIdOut, string pinOut)
        {
            var acc = _db.Accounts.AsNoTracking().FirstOrDefault(i => i.Id == accountIdOut);

            var accId = acc?.Id ?? -1;
            if (acc == null)
            {
                modelState.TryAddModelError("Account check", "Account is not found.");
            }
            else
            {
                if (_wrongPins.ContainsKey(accId) && _wrongPins[accId] > 2)
                {
                    modelState.TryAddModelError("Pin code failure", "Your card is lock because you enter wrong pin code 3 times.");
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
                            modelState.TryAddModelError("Pin code failure", "Your card is lock because you enter wrong pin code 3 times.");
                        }
                        else
                        {
                            modelState.TryAddModelError("Pin code check", "Please enter correct pin.");
                        }
                    }
                    else
                    {
                        _wrongPins[accId] = 0;
                    }
                }
            }
        }

        private void CheckConfirmAction(ModelStateDictionary modelState, string id)
        {
            if (!IsConfirmAction(id))
            {
                modelState.TryAddModelError("Action", "Action not found.");
            }
        }

        private List<Account> GetActualAccounts()
        {
            return _creditDb.GetAccounts().Where(i =>
                _timeService.CheckTerminationDate(i.TerminationDate) && i.StandardAccount != null && i.StandardAccount.Person != null)
                .ToList();
        }

        private string GetActualActionString(string id, out string additionalData)
        {
            var actionCasted = id?.ToLower();
            additionalData = null;
            if (actionCasted == WITHDRAW_ACTION || actionCasted == nameof(PhoneNumber).ToLower())
            {
                if (actionCasted == WITHDRAW_ACTION)
                {
                    additionalData = WITHDRAW_ACTION;
                }
                else
                {
                    additionalData = nameof(PhoneNumber).ToLower();
                }
                return nameof(SelectMoney);
            }
            else if (actionCasted == nameof(AccountStatus).ToLower())
            {
                return nameof(AccountStatus);
            }
            return null;
        }

        public ActionResult Logout()
        {
            return RedirectToAction(nameof(Login));
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

        public ActionResult AccountList()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public ActionResult CellularList()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public ActionResult PhoneNumber(string id)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (IsCellular(id))
            {
                return View(new AtmCellularInputViewModel { Cellular = id });
            }
            else
            {
                return RedirectToAction(nameof(CellularList));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhoneNumber(AtmCellularInputViewModel input)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            CheckCellularAndNumber(ModelState, input?.Cellular, input?.Number);

            if (ModelState.IsValid)
            {
                _phoneNumber = input.Number;
                _cellular = input.Cellular.ToLower();
                return RedirectToAction(nameof(Confirm), "Atm", new { id = nameof(PhoneNumber), });
            }
            else
            {
                return View(input);
            }
        }

        public ActionResult Confirm(string id)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (IsConfirmAction(id))
            {
                return View(new AtmConfirmViewModel { Action = id.ToLower(), });
            }
            else
            {
                return RedirectToAction(nameof(ListOfActions));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(AtmConfirmViewModel input)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            CheckAccountAndPin(ModelState, _currentAccountId == null ? -1 : (int)_currentAccountId, input.PinCode);
            CheckConfirmAction(ModelState, input.Action);

            if (ModelState.IsValid)
            {
                return RedirectToAction(GetActualActionString(
                    input.Action, out string additionalData),
                    "Atm",
                    additionalData == null ? null : new { id = additionalData });
            }
            else
            {
                return View(input);
            }
        }

        public ActionResult AddMoney()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(nameof(AddMoney), new AtmDecimalInputViewModel { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMoney(AtmDecimalInputViewModel input)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (decimal.TryParse(input.Amount, out decimal result) && result > 0)
                    {
                        var acc = _creditDb.GetAccounts().FirstOrDefault(i => i.Id == _currentAccountId);
                        acc.Money.Amount += result;
                        _db.Update(acc);
                        _db.SaveChanges();
                    }
                }
                catch
                {
                    return View("StatusFailed", "Server error happened while processing request.");
                }

                return RedirectToAction(nameof(AccountStatus));
                //return View(_db.Accounts.Include(i => i.Money).AsNoTracking().FirstOrDefault(i => i.Id == _currentAccountId)?.Money?.Amount.ToString());
            }
            else
            {
                return View(nameof(AddMoney), input);
            }
        }

        public ActionResult AccountStatus()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(nameof(AccountStatus), _db.Accounts.Include(i => i.Money).AsNoTracking().FirstOrDefault(i => i.Id == _currentAccountId)?.Money?.Amount.ToString());
        }

        public ActionResult SelectMoney(string id)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }
            if (!IsConfirmAction(id))
            {
                return RedirectToAction(nameof(ListOfActions));
            }

            return View(nameof(SelectMoney), new AtmDecimalInputViewModel {Action = id, });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectMoney(AtmDecimalInputViewModel input)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (!IsSelectMoneyAction(input.Action))
            {
                return RedirectToAction(nameof(ListOfActions));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (decimal.TryParse(input.Amount, out decimal result) && result > 0)
                    {
                        var acc = _creditDb.GetAccounts().FirstOrDefault(i => i.Id == _currentAccountId);

                        if (acc.Money.Amount >= result)
                        {
                            if (IsActionOfCellularPayment(input.Action))
                            {
                                var cellularAcc = _creditDb.GetAccounts().FirstOrDefault(i => i?.StandardAccount?.LegalEntity?.Name.ToLower() == _cellular);

                                if (cellularAcc != null)
                                {
                                    cellularAcc.Money.Amount += result;
                                    _db.Accounts.Update(cellularAcc);
                                    _db.SaveChanges();

                                    acc.Money.Amount -= result;
                                    _db.Accounts.Update(acc);
                                    _db.SaveChanges();
                                    _lastTransaction = new AtmTransactionViewModel 
                                    {
                                        Amount = result.ToString(), 
                                        Date = _timeService.CurrentTime.ToShortDateString(),
                                        Target = _cellular, 
                                    };

                                    return RedirectToAction(nameof(PrintConfirm), "Atm", input.Action);
                                }
                                else
                                {
                                    return View("StatusFailed", "Server error happened while processing request.");
                                }
                            }
                            else if (IsActionOfWithdraw(input.Action))
                            {
                                acc.Money.Amount -= result;
                                _db.Accounts.Update(acc);
                                _db.SaveChanges();

                                _lastTransaction = new AtmTransactionViewModel
                                {
                                    Amount = result.ToString(),
                                    Date = _timeService.CurrentTime.ToShortDateString(),
                                    Target = WITHDRAW_ACTION,
                                };

                                return RedirectToAction(nameof(PrintConfirm), "Atm", input.Action);
                            }
                            else
                            {
                                return View("StatusFailed", "Server error happened while processing request.");
                            }                           
                        }
                        else
                        {
                            if (IsActionOfCellularPayment(input.Action))
                            {
                                return View("CellularPayFailed");
                            }
                            else if (IsActionOfWithdraw(input.Action))
                            {
                                return View("AccountWithdrawFailed");
                            }
                            else
                            {
                                return View("StatusFailed", "Server error happened while processing request.");
                            }
                        }
                    }
                    else
                    {
                        return View("IncorrectInput");
                    }
                }
                catch
                {
                    return View("StatusFailed", "Server error happened while processing request.");
                }
            }
            else
            {
                return View(nameof(SelectMoney), input);
            }
        }

        public ActionResult PrintConfirm(string id)
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }

        public ActionResult Print()
        {
            if (_currentAccountId == null)
            {
                return RedirectToAction(nameof(Login));
            }
            return View(nameof(Print), _lastTransaction ?? new AtmTransactionViewModel());
        }

    }
}

