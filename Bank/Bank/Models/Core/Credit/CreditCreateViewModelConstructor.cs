using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank
{
    public class CreditCreateViewModelConstructor
    {
        private readonly PersonDbEntityRetriever _personDb;
        private readonly CreditDbEntityRetriever _creditDb;
        private readonly TimeService _timeService;

        public CreditCreateViewModelConstructor(CreditDbEntityRetriever creditDb, PersonDbEntityRetriever personDb, TimeService timeService)
        {
            if (creditDb == null || personDb == null || timeService == null)
            {
                throw new ArgumentNullException();
            }
            _timeService = timeService;
            _personDb = personDb;
            _creditDb = creditDb;
        }

        public CreditCreateViewModel GenerateNew(
            int personId,
            int? currencyId = null,
            int? creditTermId = null,
            int? accountId = null,
            DateTime? openDate = null)
        {
            return Generate(personId, currencyId, creditTermId, accountId, openDate);
        }

        public CreditCreateViewModel AccountChanged(CreditCreateViewModel model)
        {
            return Generate(
                model.OwnerId,
                outCurrencyId: model.CurrencyId,
                outCreditTermId: model.CreditTermId,
                outAccountId: model.AccountSourceId,
                outOpenDate: model.OpenDate);
        }

        public DateTime DateChanged(CreditCreateViewModel model)
        {
            var result = model.OpenDate.AddDays((int)_creditDb.GetInterestAccruals().First(i => i.Id == model.InterestAccrualId).TermInDays);

            return result;
        }

        public CreditCreateViewModel TermChanged(CreditCreateViewModel model)
        {
            return Generate(model.OwnerId,
                outOpenDate: model.OpenDate);
        }

        // null for absence of changes in this component
        private CreditCreateViewModel Generate(
            int outPersonId,
            int? outCurrencyId = null,
            int? outCreditTermId = null,
            int? outAccountId = null,
            DateTime? outOpenDate = null)
        {
            try
            {
                var person = GetPersonById(outPersonId);

                var allStandardAccounts = GetStandardAccountsByPersonAndAvailableCurrency(
                    person, 
                    _creditDb.GetCreditTerms().Select(i => i.Currency).Distinct(), 
                    activeOnly: true);
                var outCurrency = _creditDb.GetCreditTerms().FirstOrDefault(i => outCurrencyId == null ? true : i.CurrencyId == outCurrencyId)?.Currency
                    ?? throw new CreditCreateException(CreditCreateExceptionType.CreditNotExist);
                
                var accounts = GetAccountsByStandardAccountsAndCurrency(allStandardAccounts, outCurrency).ToList();
                var account = GetAccountByAccountsAndAccountId(accounts, outAccountId);

                var currencyList = GetCurrenciesByStandardAccounts(allStandardAccounts).ToList();
                var currency = GetCurrencyByStandardAccount(account.StandardAccount);

                var termsList = GetCreditTermsByCurrencies(new[] { currency }).ToList();
                var term = termsList.FirstOrDefault(i => outCreditTermId == null ? true : i.Id == outCreditTermId)
                    ?? throw new CreditCreateException(CreditCreateExceptionType.CreditNotExist);

                var interestAccrual = term.InterestAccrual;
                var requiredMoney = term.MinimalCredit.Amount;
                var maxMoney = term.MaximalCredit.Amount;

                var vm = new CreditCreateViewModel
                {
                    CurrencyId = currency.Id,
                    CurrencyList = currencyList,
                    InterestAccrualId = interestAccrual.Id,
                    InterestAccrualList = new List<InterestAccrual> { interestAccrual } ,
                    OpenDate = outOpenDate == null ? _timeService.CurrentTime : (DateTime)outOpenDate,
                    TerminationDate = outOpenDate == null
                                          ? _timeService.CurrentTime.AddDays((int)interestAccrual.TermInDays)
                                          : ((DateTime)outOpenDate).AddDays((int)interestAccrual.TermInDays),
                    MinimalCredit = requiredMoney,
                    CreditNumber = DbRetrieverUtils.GenerateNewCreditId(_creditDb),
                    InterestRate = term.InterestRate,
                    MoneyAmount = account.StandardAccount.Account.Money.Amount,
                    Name = "Any user-defined name here",
                    Owner = person.FirstName + " " + person.LastName,
                    OwnerId = person.Id,
                    Passport = person.Passport.Series + " " + person.Passport.Number,
                    AccountSourceList = accounts,
                    AccountSourceId = account.Id,
                    CreditTermId = term.Id,
                    CreditTermList = termsList,
                    DailyFineRate = term.DailyFineRate,
                    SelectedCredit = requiredMoney,
                    MaximalCredit = maxMoney,
                    EarlyRepaymentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(term.EarlyRepaymentAllowed),
                    IsAnnuity = OutputFormatUtils.ConvertBoolToYesNoFormat(term.IsAnnuity),
                };

                return vm;
            }
            catch (CreditCreateException)
            {
                throw;
            }
        }
        private Person GetPersonById(int id)
        {
            try
            {
                return _personDb.GetPeople().First(i => i.Id == id);
            }
            catch
            {
                throw new CreditCreateException(CreditCreateExceptionType.PersonNotExist);
            }
        }

        private IEnumerable<StandardAccount> GetStandardAccountsByPersonAndAvailableCurrency(Person person, IEnumerable<Currency> currencies, bool activeOnly = true)
        {
            if (person == null)
            {
                throw new ArgumentNullException();
            }

            var result = _creditDb.GetStandardAccounts().Where(i => i.Person == person && currencies.Contains(i.Account.Money.Currency)
                && (!activeOnly || _timeService.CheckTerminationDate(i.Account.TerminationDate))).ToList();

            if (result.Count == 0)
            {
                throw new CreditCreateException(CreditCreateExceptionType.StandardAccountsNotExist);
            }

            return result;
        }

        private IEnumerable<Account> GetAccountsByStandardAccountsAndCurrency(
            IEnumerable<StandardAccount> standardAccounts,
            Currency currency)
        {
            if (standardAccounts == null)
            {
                throw new ArgumentNullException();
            }
            if (!standardAccounts.Any())
            {
                throw new ArgumentException();
            }
            if (currency == null)
            {
                throw new CreditCreateException(CreditCreateExceptionType.AccountsOfGivenCurrencyNotExist);
            }

            try
            {
                return standardAccounts.Select(i => i.Account).GroupBy(i => i.Money.Currency).First(i => currency == i.Key).ToList();
            }
            catch
            {
                throw new CreditCreateException(CreditCreateExceptionType.AccountsOfGivenCurrencyNotExist);
            }
        }

        private Account GetAccountByAccountsAndAccountId(IEnumerable<Account> accounts, int? outAccountId)
        {
            try
            {
                return accounts.First(i => outAccountId == null ? true : outAccountId == i.Id);
            }
            catch
            {
                throw new CreditCreateException(CreditCreateExceptionType.StandardAccountsNotExist);
            }
        }

        private IEnumerable<Currency> GetCurrenciesByStandardAccounts(IEnumerable<StandardAccount> standardAccounts)
        {
            if (standardAccounts == null)
            {
                throw new ArgumentNullException();
            }
            if (!standardAccounts.Any())
            {
                throw new ArgumentException();
            }

            return _creditDb.GetCurrencies()
                .Where(i => standardAccounts.Any(j => i == j.Account.Money.Currency))
                .ToList();
        }

        private Currency GetCurrencyByStandardAccount(StandardAccount standardAccount)
        {
            if (standardAccount == null)
            {
                throw new ArgumentNullException();
            }
            return standardAccount.Account.Money.Currency;
        }

        private IEnumerable<CreditTerm> GetCreditTermsByCurrencies(IEnumerable<Currency> currencies)
        {
            var result = _creditDb.GetCreditTerms().Where(i => currencies.Contains(i.MaximalCredit.Currency)).ToList();
            if (result.Count == 0)
            {
                throw new CreditCreateException(CreditCreateExceptionType.CreditNotExist);
            }
            return result;
        }

        private IEnumerable<CreditTerm> SelectCreditTermsByInterestAccrualId(IEnumerable<CreditTerm> terms, int? outInterestAccrualId)
        {
            if (terms == null)
            {
                throw new ArgumentNullException();
            }
            var result = terms.Where(i => outInterestAccrualId == null ? true : i.InterestAccrualId == outInterestAccrualId).ToList();

            if (result.Count == 0)
            {
                throw new CreditCreateException(CreditCreateExceptionType.InterestAccrualNotFound);
            }
            return result;
        }
    }
}


