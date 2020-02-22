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
            var person = GetPersonById(outPersonId);

            //
            // check person == null  
            //

            var allStandardAccounts = GetStandardAccountsByPerson(person, activeOnly: true);

            //
            // if there 's no StandardAccount then redirect to account creation
            //

            var accounts = GetAccountsByStandardAccountsAndCurrencies(
                allStandardAccounts,
                new[] 
                { 
                    allStandardAccounts.FirstOrDefault(i => outCurrencyId == null
                                                            ? true 
                                                            : i.Account.Money.Currency.Id == outCurrencyId)
                    .Account.Money.Currency 
                })
                .ToList();

            //
            // check if there is no account
            //

            var account = accounts.FirstOrDefault(i => outAccountId == null ? true : outAccountId == i.Id);

            var currencyList = GetCurrenciesByStandardAccounts(allStandardAccounts).ToList();
            var currency = GetCurrencyByStandardAccount(currencyList, account.StandardAccount);

            var depositVariableList = GetDepositVariablesByCurrencyAndDepositGeneralId(currency, outDepositGeneralId).ToList();
            var depositVariable = depositVariableList.First();

            var depositGeneralList = GetDepositGeneralsByCurrencies(currencyList).ToList();
            var depsoitGeneral = depositGeneralList.First(i => i.DepositVariables.Contains(depositVariable));

            var coreList = GetDepositCoresByDepositVariables(depositVariableList)
                // on interest accrual changed
                .Where(i => outInterestAccrualId == null ? true : i.InterestAccrualId == outInterestAccrualId)
                .ToList();
            var core = coreList.First(i => i.DepositVariable == depositVariable);

            var interestAccrualList = GetIntersestAccrualsByDepositVariables(coreList, depositVariableList).ToList();           
            var interestAccrual = interestAccrualList.First(i => outInterestAccrualId == null ? true : i.Id == outInterestAccrualId);

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
                RequiredMoney = core.DepositVariable.MinimalDeposit.Amount,
                SelectedMoney = 0m,
                DepositNumber = OutputFormatUtils.GenerateNewDepositId(_depositDb),
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
        private Person GetPersonById(int id)
        {
            return _personDb.GetPeople().FirstOrDefault(i => i.Id == id);
        }

        private IEnumerable<StandardAccount> GetStandardAccountsByPerson(Person person, bool activeOnly = true)
        {
            return _depositDb.GetStandardAccounts().Where(i => i.Person == person 
                && (!activeOnly || _timeService.CheckActive(i.Account.TerminationDate))).ToList();
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

        private IEnumerable<DepositVariable> GetDepositVariablesByCurrencyAndDepositGeneralId(Currency currency, int? depsoitGeneralId)
        {
            return _depositDb.GetDepositVariables().Where(i => i.CurrencyId == currency.Id && 
                (depsoitGeneralId == null || i.DepositGeneralId == depsoitGeneralId)).ToList();
        }

        private IEnumerable<DepositGeneral> GetDepositGeneralsByCurrencies(IEnumerable<Currency> currencies)
        {
            return _depositDb.GetDepositGenerals().Where(i => i.DepositVariables.Any(j => currencies.Contains(j.Currency))).ToList();
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
