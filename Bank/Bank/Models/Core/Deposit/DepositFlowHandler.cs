using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class DepositFlowHandler : ISkippable
    {
        private readonly BankAppDbContext _db;
        private readonly DepositDbEntityRetriever _depositDb;
        private readonly TimeService _timeService;

        public DepositFlowHandler(DepositDbEntityRetriever depositDb, TimeService timeService, BankAppDbContext db)
        {
            _depositDb = depositDb;
            _timeService = timeService;
            _db = db;
        }

        public bool Close(int accountId, bool closedInTime)
        {
            var deposit = _depositDb.GetDepositAccounts().First(i => i.Account.Id == accountId);

            if (closedInTime)
            {
                if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
                {
                    var profit = (_timeService.CountElapsedDays(deposit.Account.OpenDate) % 30) / 365.0m
                        * (deposit.DepositCore.InterestRate / 100.0m)
                        * (deposit.Account.Money.Amount + deposit.Profit.Amount);
                    deposit.Profit.Amount += profit;
                }
                else if (!deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
                {
                    var profit = (int)deposit.DepositCore.InterestAccrual.TermInDays / 365.0m
                        * (deposit.DepositCore.InterestRate / 100.0m)
                        * (deposit.Account.Money.Amount + deposit.Profit.Amount);
                    deposit.Profit.Amount += profit;
                }
            }
            else
            {
                if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization)
                {
                    deposit.Profit.Amount = 0;
                }
            }

            var totalMoney = deposit.Account.Money.Amount + deposit.Profit.Amount;

            deposit.Account.TerminationDate = _timeService.CurrentTime;
            _db.DepositAccounts.Update(deposit);
            _db.SaveChanges();

            var bank = _depositDb.GetStandardAccounts().First(i => i.LegalEntity == _depositDb.GetLegalEntities().First(j => j.Name.Contains("BelAPB.by"))
                && i.Account.Money.Currency == deposit.Account.Money.Currency);
            bank.Account.Money.Amount -= totalMoney;
            _db.StandardAccounts.Update(bank);
            _db.SaveChanges();

            // create standard account with equal amount of money
            var standardAccount = new StandardAccount
            {
                Person = deposit.Person,
            };
            _db.StandardAccounts.Add(standardAccount);
            _db.SaveChanges();

            var money = new Money
            {
                Currency = deposit.Account.Money.Currency,
                Amount = totalMoney,
            };
            _db.Moneys.Add(money);
            _db.SaveChanges();

            var acc = new Account
            {
                Name = $"closed deposit {deposit.Account.Number}",
                Number = DbRetrieverUtils.GenerateNewStandardAccountId(_depositDb),
                OpenDate = _timeService.CurrentTime,
                StandardAccount = standardAccount,
                Money = money,
            };
            _db.Accounts.Add(acc);
            _db.SaveChanges();

            return true;
        }

        public void SkipDay()
        {
            foreach (var deposit in _depositDb.GetDepositAccounts())
            {
                if (_timeService.IsActive(deposit.Account.OpenDate, deposit.Account.TerminationDate))
                {
                    // month elapsed (for deposit with capitalization)
                    if (deposit.DepositCore.DepositVariable.DepositGeneral.WithCapitalization
                        && _timeService.IsMultipleOfMonth(deposit.Account.OpenDate))
                    {
                        var profit = (30.0m / 365.0m)
                            * (deposit.DepositCore.InterestRate / 100.0m)
                            * (deposit.Account.Money.Amount + deposit.Profit.Amount);
                        deposit.Profit.Amount += profit;
                        _db.DepositAccounts.Update(deposit);
                        _db.SaveChanges();
                    }
                    // termination date is now
                    if (_timeService.CountElapsedDays(deposit.Account.OpenDate) == (deposit.DepositCore.InterestAccrual.TermInDays ?? 365))
                    {
                        Close(deposit.Account.Id, true);
                    }
                }
            }
        }
    }
}
