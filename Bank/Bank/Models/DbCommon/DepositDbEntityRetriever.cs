using Bank.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class DepositDbEntityRetriever
    {
        public BankAppDbContext Db { get; }

        public DepositDbEntityRetriever(BankAppDbContext context)
        {
            Db = context;
        }

        public List<Account> GetAccounts() 
        {
            return Db.Accounts
                .Include(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.DepositAccount).ThenInclude(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.DepositAccount).ThenInclude(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.StandardAccount)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Currency> GetCurrencies()
        {
            return Db.Currencies
                .Include(i => i.DepositVariables)
                .Include(i => i.Moneys)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositAccount> GetDepositAccounts()
        {
            return Db.DepositAccounts
                .Include(i => i.Account).ThenInclude(i => i.Money)
                .Include(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.DepositCore).ThenInclude(i => i.InterestAccrual)
                .Include(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.Currency)
                .Include(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.Profit)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositCore> GetDepositCores()
        {
            return Db.DepositCores
                .Include(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.InterestAccrual)
                .Include(i => i.DepositAccounts)
                .OrderBy(i => i.InterestAccrualId)
                .ThenBy(i => i.DepositVariableId)
                .ToList();
        }

        public List<DepositGeneral> GetDepositGenerals()
        {
            return Db.DepositGenerals
                .Include(i => i.DepositVariables)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositVariable> GetDepositVariables()
        {
            return Db.DepositVariables
                .Include(i => i.Currency)
                .Include(i => i.DepositCores)
                .Include(i => i.DepositGeneral)
                .Include(i => i.MinimalDeposit)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<InterestAccrual> GetInterestAccruals()
        {
            return Db.InterestAccruals
                .Include(i => i.DepositCores)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<LegalEntity> GetLegalEntities()
        {
            return Db.LegalEntities
                .Include(i => i.StandardAccounts)
                .OrderBy(i => i.Id)
                .ToList(); //
        }

        public List<Money> GetMoneys()
        {
            return Db.Moneys
                .Include(i => i.Currency)
                //.Include(i => i.Account)
                //.Include(i => i.DepositVariable)
                //.Include(i => i.Transaction)
                //.Include(i => i.DepositAccount)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<StandardAccount> GetStandardAccounts()
        {
            return Db.StandardAccounts
                .Include(i => i.Account).ThenInclude(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.LegalEntity)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Transaction> GetTransactions()
        {
            return Db.Transactions
                .Include(i => i.Account)
                .Include(i => i.Amount)
                .OrderBy(i => i.Id)
                .ToList();
        }
    }
}
