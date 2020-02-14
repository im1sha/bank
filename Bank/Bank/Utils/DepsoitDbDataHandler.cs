using Bank.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank
{
    public class DepsoitDbDataHandler
    {
        private readonly BankAppDbContext _db;

        public DepsoitDbDataHandler(BankAppDbContext context)
        {
            _db = context;
        }

        public List<Account> GetAccounts() 
        {
            return _db.Accounts
                .Include(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.DepositAccount).ThenInclude(i => i.Person)
                .Include(i => i.StandardAccount).ThenInclude(i => i.Person)
                .Include(i => i.StandardAccount).ThenInclude(i => i.LegalEntity)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Currency> GetCurrencies()
        {
            return _db.Currencies
                .Include(i => i.DepositVariables).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.DepositVariables).ThenInclude(i => i.DepositCores)
                .Include(i => i.DepositVariables).ThenInclude(i => i.DepositCores).ThenInclude(i => i.InterestAccrual)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositAccount> GetDepositAccounts()
        {
            return _db.DepositAccounts
                .Include(i => i.Account).ThenInclude(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.Person)
                .Include(i => i.DepositCore).ThenInclude(i => i.DepositVariable)
                .Include(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.DepositCore).ThenInclude(i => i.InterestAccrual)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositCore> GetDepositCores()
        {
            return _db.DepositCores
                .Include(i => i.DepositVariable).ThenInclude(i => i.Currency)
                .Include(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.InterestAccrual)
                .Include(i => i.DepositAccounts).ThenInclude(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.DepositAccounts).ThenInclude(i => i.Account)
                .OrderBy(i => i.InterestAccrualId)
                .ThenBy(i => i.DepositVariableId)
                .ToList();
        }

        public List<DepositGeneral> GetDepositGenerals()
        {
            return _db.DepositGenerals
                .Include(i => i.DepositVariables).ThenInclude(i => i.Currency)
                .Include(i => i.DepositVariables).ThenInclude(i => i.MinimalDeposit)
                .Include(i => i.DepositVariables).ThenInclude(i => i.DepositCores)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<DepositVariable> GetDepositVariables()
        {
            return _db.DepositVariables
                .Include(i => i.Currency)
                .Include(i => i.DepositCores)
                .Include(i => i.DepositGeneral)
                .Include(i => i.MinimalDeposit).ThenInclude(i => i.Currency)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<InterestAccrual> GetInterestAccruals()
        {
            return _db.InterestAccruals
                .Include(i => i.DepositCores).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.Currency)
                .Include(i => i.DepositCores).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.MinimalDeposit)
                .Include(i => i.DepositCores).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<LegalEntity> GetLegalEntities()
        {
            return _db.LegalEntities
                .Include(i => i.StandardAccounts).ThenInclude(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.StandardAccounts).ThenInclude(i => i.Account).ThenInclude(i => i.StandardAccount)
                .Include(i => i.StandardAccounts).ThenInclude(i => i.Account).ThenInclude(i => i.Money)
                .OrderBy(i => i.Id)
                .ToList(); //
        }

        public List<StandardAccount> GetStandardAccounts()
        {
            return _db.StandardAccounts
                .Include(i => i.Account).ThenInclude(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.LegalEntity)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Transaction> GetTransactions()
        {
            return _db.Transactions
                .Include(i => i.Account).ThenInclude(i => i.DepositAccount).ThenInclude(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.Account).ThenInclude(i => i.DepositAccount).ThenInclude(i => i.DepositCore).ThenInclude(i => i.DepositVariable).ThenInclude(i => i.DepositGeneral)
                .Include(i => i.Account).ThenInclude(i => i.StandardAccount)
                .Include(i => i.Amount)
                .OrderBy(i => i.Id)
                .ToList();
        }
    }
}
