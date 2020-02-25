using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank
{
    public class DepositCreateViewModelConstructor
    {
        private readonly PersonDbEntityRetriever _personDb;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly TimeService _timeService;

        public DepositCreateViewModelConstructor(DepositDbEntityRetriever depositDb, PersonDbEntityRetriever personDb, TimeService timeService)
        {
            if (depositDb == null || personDb == null || timeService == null)
            {
                throw new ArgumentNullException();
            }
            _timeService = timeService;
            _personDb = personDb;
            _depositDb = depositDb;
        }
   
        //public DepositCreateViewModel CurrencyChanged(DepositCreateViewModel model)
        //{
        //    return Generate(
        //        model.OwnerId,
        //        outCurrencyId: model.CurrencyId,
        //        outDepositGeneralId: null,
        //        outAccountId: null,
        //        outInterestAccrualId: null,
        //        outOpenDate: model.OpenDate);
        //}

        //public DepositCreateViewModel DepositChanged(DepositCreateViewModel model)
        //{
        //    return Generate(
        //        model.OwnerId,
        //        outCurrencyId: model.CurrencyId,
        //        outDepositGeneralId: model.DepositGeneralId,
        //        outAccountId: null,
        //        outInterestAccrualId: null,
        //        outOpenDate: model.OpenDate);
        //}

        public DepositCreateViewModel GenerateNew(
            int personId,
            int? currencyId = null,
            int? depositGeneralId = null,
            int? accountId = null,
            int? interestAccrualId = null,
            DateTime? openDate = null)
        {
            return Generate(personId, currencyId, depositGeneralId, accountId, interestAccrualId, openDate);
        }

        public DepositCreateViewModel AccountChanged(DepositCreateViewModel model)
        {
            return Generate(
                model.OwnerId, 
                outCurrencyId: model.CurrencyId,
                outDepositGeneralId: model.DepositGeneralId,
                outAccountId: model.AccountSourceId,
                outInterestAccrualId: model.InterestAccrualId,
                outOpenDate: model.OpenDate);
        }

        public DateTime DateChanged(DepositCreateViewModel model)
        {
            var result = model.OpenDate.AddDays((int)_depositDb.GetInterestAccruals().First(i => i.Id == model.InterestAccrualId).TermInDays);

            return result;
        }

        public DepositCreateViewModel TermChanged(DepositCreateViewModel model)
        {
            return Generate(model.OwnerId, 
                outInterestAccrualId: model.InterestAccrualId, 
                outOpenDate: model.OpenDate);
        }
    
        // null for absence of changes in this component
        private DepositCreateViewModel Generate(
            int outPersonId,  
            int? outCurrencyId = null,    
            int? outDepositGeneralId = null,
            int? outAccountId = null, 
            int? outInterestAccrualId = null,
            DateTime? outOpenDate = null)
        {
            try
            {
                var person = GetPersonById(outPersonId);
         
                var allStandardAccounts = GetStandardAccountsByPerson(person, activeOnly: true);
                var outCurrency = allStandardAccounts.FirstOrDefault(i => outCurrencyId == null
                    ? true : i?.Account?.Money?.Currency?.Id == outCurrencyId)?.Account?.Money?.Currency;

                var accounts = GetAccountsByStandardAccountsAndCurrency(allStandardAccounts, outCurrency).ToList();
                var account = GetAccountByAccountsAndAccountId(accounts, outAccountId);

                var currencyList = GetCurrenciesByStandardAccounts(allStandardAccounts).ToList();
                var currency = GetCurrencyByStandardAccount(account.StandardAccount);

                var depositVariableList = GetDepositVariablesByCurrencyAndDepositGeneralId(currency, outDepositGeneralId).ToList();
                var depositVariable = depositVariableList.First();

                var depositGeneralList = GetDepositGeneralsByCurrencies(currencyList).ToList();
                var depsoitGeneral = depositGeneralList.First(i => i.DepositVariables.Contains(depositVariable));

                var coreList = SelectDepositCoresByInterestAccrualId(
                    GetDepositCoresByDepositVariables(depositVariableList),
                    outInterestAccrualId)
                    .ToList();
                var core = coreList.First(i => i.DepositVariable == depositVariable);

                var interestAccrualList = GetIntersestAccrualsByCores(coreList).ToList();           
                var interestAccrual = interestAccrualList.First(i => outInterestAccrualId == null ? true : i.Id == outInterestAccrualId);

                var requiredMoney = core.DepositVariable.MinimalDeposit.Amount;

                var vm = new DepositCreateViewModel
                {
                    CurrencyId = currency.Id,
                    CurrencyList = currencyList.Select(i => new CurrencyViewModel { Id = i.Id, Name = i.Name, }).ToList(),
                    DepositGeneralId = depsoitGeneral.Id,
                    DepositGeneralList = depositGeneralList.Select(i => new DepositGeneralViewModel { Id = i.Id, Name = i.Name, }).ToList(),
                    InterestAccrualId = interestAccrual.Id,
                    InterestAccrualList = interestAccrualList.Select(i => new InterestAccrualViewModel { Id = i.Id, Name = i.Name, }).ToList(),
                    IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.IsRevocable),
                    WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.WithCapitalization),
                    ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.ReplenishmentAllowed),
                    OpenDate = outOpenDate == null ? _timeService.CurrentTime : (DateTime) outOpenDate,
                    TerminationDate = outOpenDate == null 
                                          ? _timeService.CurrentTime.AddDays((int)interestAccrual.TermInDays)
                                          : ((DateTime)outOpenDate).AddDays((int)interestAccrual.TermInDays),
                    RequiredMoney = requiredMoney,
                    SelectedMoney = requiredMoney,
                    DepositNumber = DbRetrieverUtils.GenerateNewDepositId(_depositDb),
                    InterestRate = core.InterestRate,
                    MoneyAmount = account.StandardAccount.Account.Money.Amount,
                    Name = "Any user-defined name here",
                    Owner = person.FirstName + " " + person.LastName,
                    OwnerId = person.Id,
                    Passport = person.Passport.Series + " " + person.Passport.Number,
                    AccountSourceList = accounts.Select(i => new AccountViewModel { Id = i.Id, Number = i.Number, }).ToList(),
                    AccountSourceId = account.Id,
                };

                return vm;
            }
            catch (DepositCreateException)
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
                throw new DepositCreateException(DepositCreateExceptionType.PersonNotExist);
            }
        }

        private IEnumerable<StandardAccount> GetStandardAccountsByPerson(Person person, bool activeOnly = true)
        {
            if (person == null)
            {
                throw new ArgumentNullException();
            }

            var result = _depositDb.GetStandardAccounts().Where(i => i.Person == person 
                && (!activeOnly || _timeService.CheckActive(i.Account.TerminationDate))).ToList();

            if (result.Count == 0)
            {
                throw new DepositCreateException(DepositCreateExceptionType.StandardAccountsNotExist);
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
                throw new DepositCreateException(DepositCreateExceptionType.AccountsOfGivenCurrencyNotExist);
            }

            try
            {
                return standardAccounts.Select(i => i.Account).GroupBy(i => i.Money.Currency).First(i => currency == i.Key).ToList();
            }
            catch 
            {
                throw new DepositCreateException(DepositCreateExceptionType.AccountsOfGivenCurrencyNotExist);
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
                throw new DepositCreateException(DepositCreateExceptionType.StandardAccountsNotExist);
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

            return _depositDb.GetCurrencies()
                .Where(i => i.DepositVariables.Any() && standardAccounts.Any(j => i == j.Account.Money.Currency))
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

        private IEnumerable<DepositVariable> GetDepositVariablesByCurrencyAndDepositGeneralId(Currency currency, int? depsoitGeneralId)
        {
            if (currency == null)
            {
                throw new ArgumentNullException();
            }
            var result = _depositDb.GetDepositVariables().Where(i => i.CurrencyId == currency.Id
                && (depsoitGeneralId == null || i.DepositGeneralId == depsoitGeneralId)).ToList();

            if (result.Count == 0)
            {
                throw new DepositCreateException(DepositCreateExceptionType.DepositNotExist);
            }

            return result;
        }

        private IEnumerable<DepositGeneral> GetDepositGeneralsByCurrencies(IEnumerable<Currency> currencies)
        {
            var result = _depositDb.GetDepositGenerals().Where(i => i.DepositVariables.Any(j => currencies.Contains(j.Currency))).ToList();
            if (result.Count == 0)
            {
                throw new DepositCreateException(DepositCreateExceptionType.DepositNotExist);
            }
            return result;
        }

        private IEnumerable<DepositCore> SelectDepositCoresByInterestAccrualId(IEnumerable<DepositCore> cores, int? outInterestAccrualId) 
        {
            if (cores == null)
            {
                throw new ArgumentNullException();
            }
            var result = cores.Where(i => outInterestAccrualId == null ? true : i.InterestAccrualId == outInterestAccrualId).ToList();

            if (result.Count == 0)
            {
                throw new DepositCreateException(DepositCreateExceptionType.InterestAccrualNotFound);
            }
            return result;
        }

        private IEnumerable<DepositCore> GetDepositCoresByDepositVariables(IEnumerable<DepositVariable> depositVariableList)
        {
            var result = _depositDb.GetDepositCores().Where(i => depositVariableList.Contains(i.DepositVariable)).ToList();
            if (result.Count == 0)
            {
                throw new DepositCreateException(DepositCreateExceptionType.DepositNotExist);
            }
            return result;
        }

        private IEnumerable<InterestAccrual> GetIntersestAccrualsByCores(
            IEnumerable<DepositCore> coreList)
        {
            if (coreList == null)
            {
                throw new ArgumentNullException();
            }
            if (!coreList.Any())
            {
                throw new ArgumentException();
            }

            return coreList.Select(i => i.InterestAccrual).Distinct().ToList();
        }

        private void ChangeAccountsUsingRequiredMoneyAmount(ref List<Account> accounts, ref Account account, decimal requiredMoney, int? outAccountId)
        {
            accounts = accounts.Where(i => i.Money.Amount >= requiredMoney).ToList();
            if (!accounts.Contains(account) && outAccountId != null)
            {
                throw new DepositCreateException(DepositCreateExceptionType.NotEnoughOfMoney);
            }
            else
            {
                account = GetAccountByAccountsAndAccountId(accounts, outAccountId);
            }
        }
    }
}


