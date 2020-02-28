using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bank.Controllers
{
    public class CreditController : Controller
    {
        private readonly ILogger<CreditController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly CreditDbEntityRetriever _creditDb;
        private readonly PersonDbEntityRetriever _personDb;
        private readonly BankAppDbContext _db;
        private readonly TimeService _timeService;
        private readonly FlowService _flowService;

        public CreditController(BankAppDbContext context, ILogger<CreditController> logger, LinkGenerator linkGenerator, TimeService timeService, FlowService flowService/*, CreditDbEntityRetriever creditDb, PersonDbEntityRetriever personDb*/)
        {
            _db = context;
            _creditDb = new CreditDbEntityRetriever(context);
            _personDb = new PersonDbEntityRetriever(context);
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
                _flowService.SkipDay();
            }
            else if (skipMonth)
            {
                for (int i = 0; i < 30; i++)
                {
                    _flowService.SkipDay();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Credit
        //      Credit/index/5
        // id == person id
        public ActionResult Index(int? id)
        {
            var accs = _creditDb.GetCreditAccounts().Where(i => id == null ? true : i.PersonId == id).ToList();

            var models = accs.Select(i => new CreditIndexViewModel
            {
                AccountId = i.Account.Id,
                AccountName = i.Account.Name,
                AccountNumber = i.Account.Number,
                Currency = i.CreditTerm.Currency.Name,
                CreditName = i.CreditTerm.Name,
                Id = i.Id,
                InterestRate = i.CreditTerm.InterestRate,
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(i.Account.OpenDate, i.Account.TerminationDate)),
                MoneyAmount = i.Account.Money.Amount,
                OpenDate = i.Account.OpenDate,
                Owner = i.Person.FirstName + " " + i.Person.LastName,
                OwnerId = i.Person.Id,
                Passport = i.Person.Passport.Series + i.Person.Passport.Number,
                Term = i.CreditTerm.InterestAccrual.Name,
                TerminationDate = i.Account.TerminationDate,
                DailyFineRate = i.CreditTerm.DailyFineRate,
                EarlyRepaymentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(i.CreditTerm.EarlyRepaymentAllowed),
                Fine = i.Fine.Amount,
                IsAnnuity = OutputFormatUtils.ConvertBoolToYesNoFormat(i.CreditTerm.IsAnnuity),
                MaximalCredit = i.CreditTerm.MaximalCredit.Amount,
                MinimalCredit = i.CreditTerm.MinimalCredit.Amount,
                PaidFinePart = i.PaidFinePart.Amount,
                PaidMainPart = i.PaidMainPart.Amount,
                PaidPercentagePart = i.PaidPercentagePart.Amount,
                Percentage = i.Percentage.Amount,
                NextPayment = CalculateNextPayment(i),
                RequiredToCloseCredit = CalculateAmountRequiredToCloseCredit(i),
            }).ToList();

            return View(models);
        }

        private decimal CalculateNextPayment(CreditAccount creditAccount)
        {
            return 0m;
        }

        private decimal CalculateAmountRequiredToCloseCredit(CreditAccount creditAccount)
        {
            return 0m;
        }

        // GET: Credit/Details/5
        public ActionResult Details(int id)
        {
            var creditAcc = _creditDb.GetCreditAccounts().First(j => j.Id == id);

            var model = new CreditIndexViewModel
            {
                AccountId = creditAcc.Account.Id,
                AccountName = creditAcc.Account.Name,
                AccountNumber = creditAcc.Account.Number,
                Currency = creditAcc.CreditTerm.Currency.Name,
                CreditName = creditAcc.CreditTerm.Name,
                Id = creditAcc.Id,
                InterestRate = creditAcc.CreditTerm.InterestRate,
                IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(creditAcc.Account.OpenDate, creditAcc.Account.TerminationDate)),
                MoneyAmount = creditAcc.Account.Money.Amount,
                OpenDate = creditAcc.Account.OpenDate,
                Owner = creditAcc.Person.FirstName + " " + creditAcc.Person.LastName,
                OwnerId = creditAcc.Person.Id,
                Passport = creditAcc.Person.Passport.Series + creditAcc.Person.Passport.Number,
                Term = creditAcc.CreditTerm.InterestAccrual.Name,
                TerminationDate = creditAcc.Account.TerminationDate,
                DailyFineRate = creditAcc.CreditTerm.DailyFineRate,
                EarlyRepaymentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(creditAcc.CreditTerm.EarlyRepaymentAllowed),
                Fine = creditAcc.Fine.Amount,
                IsAnnuity = OutputFormatUtils.ConvertBoolToYesNoFormat(creditAcc.CreditTerm.IsAnnuity),
                MaximalCredit = creditAcc.CreditTerm.MaximalCredit.Amount,
                MinimalCredit = creditAcc.CreditTerm.MinimalCredit.Amount,
                PaidFinePart = creditAcc.PaidFinePart.Amount,
                PaidMainPart = creditAcc.PaidMainPart.Amount,
                PaidPercentagePart = creditAcc.PaidPercentagePart.Amount,
                Percentage = creditAcc.Percentage.Amount,
                NextPayment = CalculateNextPayment(creditAcc),
                RequiredToCloseCredit = CalculateAmountRequiredToCloseCredit(creditAcc),
                AccountNumberOfSourceStandardAccount = creditAcc.SourceStandardAccount.Account.Number,
            };

            return View(model);
        }

        #region api of interaction with form of credit creation

        [HttpPost]
        public ActionResult AccountChanged(CreditCreateViewModel model)
        {
            var result = new CreditCreateViewModelConstructor(_creditDb, _personDb, _timeService).AccountChanged(model);

            return Json(result.MoneyAmount);
        }

        [HttpPost]
        public ActionResult DateChanged(CreditCreateViewModel model)
        {
            return Json(new CreditCreateViewModelConstructor(_creditDb, _personDb, _timeService).DateChanged(model));
        }

        [HttpPost]
        public ActionResult TermChanged(CreditCreateViewModel model)
        {
            var result = new CreditCreateViewModelConstructor(_creditDb, _personDb, _timeService).TermChanged(model);

            return Json(new { openDate = result.OpenDate, terminationDate = result.TerminationDate, interestRate = result.InterestRate, });
        }

        #endregion

        // GET: Credit/Create?personId=5&currencyId=1
        public ActionResult Create(
            [FromQuery]int? personId,
            [FromQuery]int? currencyId = null,
            [FromQuery]int? creditTermId = null,
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
                var result = new CreditCreateViewModelConstructor(_creditDb, _personDb, _timeService)
                    .GenerateNew((int)personId, currencyId, creditTermId, null, openDate);
                return View(result);
            }
            catch (CreditCreateException e)
            {
                switch (e.Reason)
                {
                    case CreditCreateExceptionType.StandardAccountsNotExist:
                    case CreditCreateExceptionType.AccountsOfGivenCurrencyNotExist:
                        return View(
                            "StatusStandardAccountsNotFound",
                            _linkGenerator.GetUriByAction(
                                HttpContext,
                                nameof(StandardAccountController.Create),
                                "StandardAccount",
                                new { id = personId, isPerson = true }));
                    case CreditCreateExceptionType.PersonNotExist:
                    case CreditCreateExceptionType.CreditNotExist:
                    case CreditCreateExceptionType.InterestAccrualNotFound:
                    default:
                        return View("StatusNotFound");
                }
            }
        }

        // POST: Credit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreditCreateViewModel input)
        {
            var personId = input.OwnerId;
            var currencyId = input.CurrencyId;
            var termId = input.CreditTermId;
            var accountSourceId = input.AccountSourceId;
            var openDate = input.OpenDate;
            var selectedMoneyAmount = input.SelectedCredit;
            var accName = input.Name;
            var bankAccount = _creditDb.GetLegalEntities().First().StandardAccounts.First(j => j.Account.Money.CurrencyId == currencyId).Account;

            try
            {
                if (!CheckMoneyAmountUsingBankAccount(
                        currencyId,
                        termId,
                        bankAccount.Id,
                        bankAccount.StandardAccount.Account.Money.Amount,
                        selectedMoneyAmount))
                {
                    ModelState.TryAddModelError("Money is out of bounds", "You should enter amount of money" +
                        " that is less than max amount and not less than min possible amount.");
                }
                if (!CheckOpenDate(openDate))
                {
                    ModelState.TryAddModelError("Open date in the past", "Deposit open date should be not less than current date.");
                }

                // use on else condition
                var vm = new CreditCreateViewModelConstructor(_creditDb, _personDb, _timeService)
                    .GenerateNew(personId, currencyId, termId, accountSourceId, openDate);

                if (ModelState.IsValid)
                {
                    //
                    // use input data here, not vm
                    //

                    var sourceAccount = _creditDb.GetAccounts().First(i => i.Id == accountSourceId);

                    // credit account all the money instances
                    var moneys = Enumerable.Repeat(
                        new Money
                        {
                            Currency = _creditDb.GetCurrencies().First(i => i.Id == currencyId),
                            Amount = 0,
                        },
                        5).ToList();

                    _db.Moneys.AddRange(moneys);
                    _db.SaveChanges();

                    var selectMoney = new Money
                    {
                        Currency = _creditDb.GetCurrencies().First(i => i.Id == currencyId),
                        Amount = selectedMoneyAmount,
                    };
                    _db.Moneys.Add(selectMoney);
                    _db.SaveChanges();

                    // create ctedit
                    var credit = new CreditAccount
                    {
                        Person = _personDb.GetPeople().First(i => i.Id == personId),
                        CreditTerm = _creditDb.GetCreditTerms().First(i => i.Id == termId),
                        Fine = moneys[0],
                        Percentage = moneys[1],
                        PaidFinePart = moneys[2],
                        PaidMainPart = moneys[3],
                        PaidPercentagePart = moneys[4],
                        SourceStandardAccount = sourceAccount.StandardAccount,
                    };
                    _db.CreditAccounts.Add(credit);
                    _db.SaveChanges();

                    var accForCredit = new Account
                    {
                        CreditAccount = credit,
                        Name = accName,
                        Number = DbRetrieverUtils.GenerateNewCreditId(_creditDb),
                        OpenDate = openDate,
                        TerminationDate = null,
                        Money = selectMoney,
                    };
                    _db.Accounts.Add(accForCredit);
                    _db.SaveChanges();

                    // decrease amount of money at bank account
                    bankAccount.StandardAccount.Account.Money.Amount -= selectedMoneyAmount;
                    _db.Accounts.Update(bankAccount);
                    _db.SaveChanges();


                    // create standard account with required amount of money
                    var standardAccount = new StandardAccount
                    {
                        Person = _personDb.GetPeople().First(i => i.Id == personId),
                    };
                    _db.StandardAccounts.Add(standardAccount);
                    _db.SaveChanges();

                    var standardAccountMoney = new Money
                    {
                        Currency = _creditDb.GetCurrencies().First(i => i.Id == currencyId),
                        Amount = selectedMoneyAmount,
                    };
                    _db.Moneys.Add(standardAccountMoney);
                    _db.SaveChanges();

                    var acc = new Account
                    {
                        Name = accName,
                        Number = DbRetrieverUtils.GenerateNewStandardAccountId(_creditDb),
                        OpenDate = _timeService.CurrentTime,
                        Money = standardAccountMoney,
                        StandardAccount = standardAccount,
                    };
                    _db.Accounts.Add(acc);
                    _db.SaveChanges();

                    return View("StatusSucceeded", "Credit creation succeeded.");
                }
                else
                {
                    return View(vm);
                }
            }
            catch
            {
                return View("StatusFailed", "Credit creation failed.");
            }
        }

        private bool CheckOpenDate(DateTime openDate)
        {
            return openDate >= _timeService.CurrentTime;
        }

        private bool CheckMoneyAmountUsingBankAccount(int currencyId, int creditTermId, int accountSourceId, decimal moneyAmountAtBank, decimal moneyAmountRequired)
        {
            try
            {
                var account = _creditDb.GetAccounts().First(i => i.Id == accountSourceId);

                if (account.Money.CurrencyId != currencyId)
                {
                    return false;
                }
                if (account.Money.Amount < moneyAmountAtBank)
                {
                    return false;
                }

                return _creditDb.GetCreditTerms().First(i => i.Id == creditTermId).MinimalCredit.Amount <= moneyAmountRequired
                    && _creditDb.GetCreditTerms().First(i => i.Id == creditTermId).MaximalCredit.Amount >= moneyAmountRequired;
            }
            catch
            {
                return false;
            }
        }

        // GET: Credit/Delete/5
        // id == creditAccount id
        public ActionResult Delete(int id)
        {
            try
            {
                var creditAcc = _creditDb.GetCreditAccounts().First(j => j.Id == id);

                if (creditAcc == null)
                {
                    return View("StatusNotFound");
                }
                if (!_timeService.CheckTerminationDate(creditAcc.Account.TerminationDate))
                {
                    return View("StatusFailed", "Account is closed.");
                }

                var model = new CreditIndexViewModel
                {
                    AccountId = creditAcc.Account.Id,
                    AccountName = creditAcc.Account.Name,
                    AccountNumber = creditAcc.Account.Number,
                    Currency = creditAcc.CreditTerm.Currency.Name,
                    CreditName = creditAcc.CreditTerm.Name,
                    Id = creditAcc.Id,
                    InterestRate = creditAcc.CreditTerm.InterestRate,
                    IsActive = OutputFormatUtils.ConvertBoolToYesNoFormat(_timeService.IsActive(creditAcc.Account.OpenDate, creditAcc.Account.TerminationDate)),
                    MoneyAmount = creditAcc.Account.Money.Amount,
                    OpenDate = creditAcc.Account.OpenDate,
                    Owner = creditAcc.Person.FirstName + " " + creditAcc.Person.LastName,
                    OwnerId = creditAcc.Person.Id,
                    Passport = creditAcc.Person.Passport.Series + creditAcc.Person.Passport.Number,
                    Term = creditAcc.CreditTerm.InterestAccrual.Name,
                    TerminationDate = creditAcc.Account.TerminationDate,
                    DailyFineRate = creditAcc.CreditTerm.DailyFineRate,
                    EarlyRepaymentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(creditAcc.CreditTerm.EarlyRepaymentAllowed),
                    Fine = creditAcc.Fine.Amount,
                    IsAnnuity = OutputFormatUtils.ConvertBoolToYesNoFormat(creditAcc.CreditTerm.IsAnnuity),
                    MaximalCredit = creditAcc.CreditTerm.MaximalCredit.Amount,
                    MinimalCredit = creditAcc.CreditTerm.MinimalCredit.Amount,
                    PaidFinePart = creditAcc.PaidFinePart.Amount,
                    PaidMainPart = creditAcc.PaidMainPart.Amount,
                    PaidPercentagePart = creditAcc.PaidPercentagePart.Amount,
                    Percentage = creditAcc.Percentage.Amount,
                    NextPayment = CalculateNextPayment(creditAcc),
                    RequiredToCloseCredit = CalculateAmountRequiredToCloseCredit(creditAcc),
                    AccountNumberOfSourceStandardAccount = creditAcc.SourceStandardAccount.Account.Number,
                };

                return View(model);
            }
            catch (Exception)
            {
                return View("StatusFailed", "Credit close failed.");
            }
        }

        // POST: Credit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DepositIndexViewModel model)
        {
            try
            {
                //var dep = _depositDb.GetDepositAccounts().First(i => i.Id == model.Id);
                //if (!_timeService.CheckTerminationDate(dep.Account.TerminationDate))
                //{
                //    return View("StatusFailed", "Account is closed.");
                //}

                //CloseDeposit(dep, false);

                return View("StatusSucceeded", "Credit close succeeded.");
            }
            catch
            {
                return View("StatusFailed", "Credit close failed.");
            }
        }
    }
}

