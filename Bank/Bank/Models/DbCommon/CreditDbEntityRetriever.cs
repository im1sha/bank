using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class CreditDbEntityRetriever
    {
        public BankAppDbContext Db { get; }

        public CreditDbEntityRetriever(BankAppDbContext context)
        {
            Db = context;
        }

        public List<Account> GetAccounts()
        {
            return Db.Accounts
                .Include(i => i.Money).ThenInclude(i => i.Currency)
                .Include(i => i.CreditAccount).ThenInclude(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.CreditAccount).ThenInclude(i => i.CreditTerm)
                .Include(i => i.StandardAccount).ThenInclude(i => i.Person)
                .Include(i => i.StandardAccount).ThenInclude(i => i.LegalEntity)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Currency> GetCurrencies()
        {
            return Db.Currencies
                .Include(i => i.Moneys)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<CreditAccount> GetCreditAccounts()
        {
            return Db.CreditAccounts
                .Include(i => i.Account).ThenInclude(i => i.Money)
                .Include(i => i.Person).ThenInclude(i => i.Passport)
                .Include(i => i.CreditTerm).ThenInclude(i => i.InterestAccrual)
                .Include(i => i.CreditTerm).ThenInclude(i => i.Currency)
                .Include(i => i.CreditTerm).ThenInclude(i => i.MaximalCredit)
                .Include(i => i.CreditTerm).ThenInclude(i => i.MinimalCredit)
                .Include(i => i.SourceStandardAccount).ThenInclude(i => i.Account).ThenInclude(i => i.Money)
                .Include(i => i.Fine)
                .Include(i => i.PaidFinePart)
                .Include(i => i.PaidMainPart)
                .Include(i => i.Main)
                .Include(i => i.PaidPercentagePart)
                .Include(i => i.Percentage)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<CreditTerm> GetCreditTerms()
        {
            return Db.CreditTerms
                .Include(i => i.InterestAccrual)
                .Include(i => i.CreditAccounts)
                .Include(i => i.MinimalCredit)
                .Include(i => i.MaximalCredit)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<InterestAccrual> GetInterestAccruals()
        {
            return Db.InterestAccruals
                .Include(i => i.CreditTerms)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<LegalEntity> GetLegalEntities()
        {
            return Db.LegalEntities
                .Include(i => i.StandardAccounts).ThenInclude(i => i.Account).ThenInclude(i => i.Money)
                .OrderBy(i => i.Id)
                .ToList();
        }

        public List<Money> GetMoneys()
        {
            return Db.Moneys
                .Include(i => i.Currency)
                .Include(i => i.Account)
                .Include(i => i.CreditAccountFine)
                .Include(i => i.CreditAccountPaidFinePart)
                .Include(i => i.CreditAccountPaidMainPart)
                .Include(i => i.CreditAccountPaidPercentagePart)
                .Include(i => i.CreditAccountPercentage)
                .Include(i => i.CreditTermMaximalCredit)
                .Include(i => i.CreditTermMinimalCredit)
                .Include(i => i.Transaction)
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
