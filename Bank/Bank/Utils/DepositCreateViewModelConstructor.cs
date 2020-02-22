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
   
        public DepositCreateViewModel CurrencyChanged(DepositCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public DepositCreateViewModel AccountChanged(DepositCreateViewModel model)
        {
            return Generate(model.OwnerId, outAccountId: model.StandardAccountSourceId);
        }

        public DepositCreateViewModel DepositChanged(DepositCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        //public DepositCreateViewModel MoneyChanged(DepositCreateViewModel model)
        //{
        //    throw new NotImplementedException();
        //}

        public DateTime DateChanged(DepositCreateViewModel model)
        {
            var result = model.OpenDate.AddDays((int)_depositDb.GetInterestAccruals().First(i => i.Id == model.InterestAccrualId).TermInDays);

            return result;
            //if (_timeService.CheckActive(result))
            //{
            //    return result;
            //}
            //return _timeService.CurrentTime;
        }

        public DepositCreateViewModel TermChanged(DepositCreateViewModel model)
        {
            return Generate(model.OwnerId, outInterestAccrualId: model.InterestAccrualId);
        }

        public DepositCreateViewModel Generate(int personId)   
        {
            return Generate(personId);
        }

        // null for absence of changes in this component
        private DepositCreateViewModel Generate(int outPersonId, int? outInterestAccrualId = null, int? outAccountId = null)
        {
            var person = GetPersonById(outPersonId);
            var standardAccounts = GetStandardAccountsByPerson(person, activeOnly: true);

            var accounts = GetAccountsByStandardAccountsAndCurrencies(
                standardAccounts,
                new[] { standardAccounts.First().Account.Money.Currency }).ToList();
            var standardAccount = accounts.First().StandardAccount;

            var currencyList = GetCurrenciesByStandardAccounts(standardAccounts).ToList();
            var currency = GetCurrencyByStandardAccount(currencyList, standardAccount);

            var depositVariableList = GetDepositVariablesByCurrency(currency).ToList();
            var depositVariable = depositVariableList.First();

            var depositGeneralList = GetDepositGeneralsByDepositVariables(depositVariableList).ToList();
            var depsoitGeneral = depositGeneralList.First(i => i.DepositVariables.Contains(depositVariable));

            var coreList = GetDepositCoresByDepositVariables(depositVariableList)
                // on interest accrual changed
                .Where(i => outInterestAccrualId == null ? true : i.InterestAccrualId == outInterestAccrualId)
                .ToList();
            var core = coreList.First(i => i.DepositVariable == depositVariable);

            var interestAccrualList = GetIntersestAccrualsByDepositVariables(coreList, depositVariableList).ToList();           
            var interestAccrual = interestAccrualList.First();

            var vm = new DepositCreateViewModel
            {
                CurrencyId = currency.Id,
                CurrencyList = currencyList,
                DepositGeneralId = depsoitGeneral.Id,
                DepositGeneralList = depositGeneralList,
                InterestAccrualId = interestAccrual.Id,
                InterestAccrualList = interestAccrualList,
                IsRevocable = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.IsRevocable),
                WithCapitalization = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.WithCapitalization),
                ReplenishmentAllowed = OutputFormatUtils.ConvertBoolToYesNoFormat(depsoitGeneral.ReplenishmentAllowed),
                OpenDate = _timeService.CurrentTime,
                TerminationDate = _timeService.CurrentTime.AddDays((int)interestAccrualList.First(i => i == interestAccrual).TermInDays),
                RequiredMoney = core.DepositVariable.MinimalDeposit.Amount,
                SelectedMoney = 0m,
                DepositNumber = OutputFormatUtils.GenerateNewDepositId(_depositDb),
                InterestRate = core.InterestRate,
                MoneyAmount = standardAccount.Account.Money.Amount,
                Name = "Any user-defined name here",
                Owner = person.FirstName + " " + person.LastName,
                OwnerId = person.Id,
                Passport = person.Passport.Series + " " + person.Passport.Number,
                StandardAccountSourceList = accounts,
                StandardAccountSourceId = accounts.First().Id,
            };

            return vm;
        }
        private Person GetPersonById(int id)
        {
            return _personDb.GetPeople().First(i => i.Id == id);
        }

        private IEnumerable<StandardAccount> GetStandardAccountsByPerson(Person person, bool activeOnly = true)
        {
            return _depositDb.GetStandardAccounts().Where(i => i.Person == person && (!activeOnly || _timeService.CheckActive(i.Account.TerminationDate))).ToList();
        }

        private IEnumerable<Account> GetAccountsByStandardAccountsAndCurrencies(
            IEnumerable<StandardAccount> standardAccounts,
            IEnumerable<Currency> currencies)
        {
            return standardAccounts.Select(i => i.Account).GroupBy(i => i.Money.Currency).FirstOrDefault(i => currencies.Contains(i.Key)).ToList();
        }

        private IEnumerable<Currency> GetCurrenciesByStandardAccounts(IEnumerable<StandardAccount> standardAccounts)
        {
            return _depositDb.GetCurrencies()
                .Where(i => i.DepositVariables.Any() && standardAccounts.Any(j => i == j.Account.Money.Currency))
                .ToList();
        }

        private Currency GetCurrencyByStandardAccount(IEnumerable<Currency> availableCurrencies, StandardAccount standardAccount)
        {
            return availableCurrencies?.FirstOrDefault(i => i.Id == standardAccount.Account.Money.CurrencyId);
        }

        private IEnumerable<DepositVariable> GetDepositVariablesByCurrency(Currency currency)
        {
            return _depositDb.GetDepositVariables().Where(i => i.CurrencyId == currency.Id).ToList();
        }

        private IEnumerable<DepositGeneral> GetDepositGeneralsByDepositVariables(IEnumerable<DepositVariable> depositVariableList)
        {
            return _depositDb.GetDepositGenerals().Where(i => i.DepositVariables.Any(j => depositVariableList.Contains(j))).ToList();
        }

        private IEnumerable<DepositCore> GetDepositCoresByDepositVariables(IEnumerable<DepositVariable> depositVariableList)
        {
            return _depositDb.GetDepositCores()
                .Where(i => depositVariableList.Contains(i.DepositVariable)).ToList();
        }

        private IEnumerable<InterestAccrual> GetIntersestAccrualsByDepositVariables(
            IEnumerable<DepositCore> coreList,
            IEnumerable<DepositVariable> depositVariableList)
        {
            return coreList.Where(i => depositVariableList.Contains(i.DepositVariable))
                 .Select(i => i.InterestAccrual).Distinct().ToList();
        }
    }
}
