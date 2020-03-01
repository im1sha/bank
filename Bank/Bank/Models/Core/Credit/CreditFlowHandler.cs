using System.Linq;

namespace Bank.Models
{
    public class CreditFlowHandler : ISkippable
    {
        private readonly BankAppDbContext _db;
        private readonly CreditDbEntityRetriever _creditDb;
        private readonly TimeService _timeService;

        public CreditFlowHandler(CreditDbEntityRetriever creditDb, TimeService timeService, BankAppDbContext db)
        {
            _creditDb = creditDb;
            _timeService = timeService;
            _db = db;
        }

        // close in time == !prediction 
        public bool Close(int sourceId, bool closedInTime)
        {
            bool prediction = !closedInTime;
            var credit = _creditDb.GetCreditAccounts().First(i => i.Account.Id == sourceId);
            var paymentCalculator = new CreditPaymentCalculator(credit, _timeService);

            if (paymentCalculator.CheckClosePossibilityByDate(prediction))
            {
                var requiredMoney = paymentCalculator.GetPayment(false);
                if (requiredMoney.Fines > 0 || requiredMoney.Main > 0 || requiredMoney.Percents > 0)
                {
                    if (credit.SourceStandardAccount.Account.Money.Amount >= requiredMoney.Fines + requiredMoney.Main + requiredMoney.Percents)
                    {
                        SubtractMoney(credit, requiredMoney);
                        return true;
                    }
                }
            }
            else
            {
                var requiredMoney = paymentCalculator.RequiredToCloseCreditPrediction();
                if (requiredMoney.Fines > 0 || requiredMoney.Main > 0 || requiredMoney.Percents > 0)
                {
                    if (credit.SourceStandardAccount.Account.Money.Amount >= requiredMoney.Fines + requiredMoney.Main + requiredMoney.Percents)
                    {
                        SubtractMoney(credit, requiredMoney);
                        return true;
                    }
                }
            }

            return false;
        }


        private void SubtractMoney(CreditAccount credit, (decimal Main, decimal Percents, decimal Fines) requiredMoney)
        {
            credit.SourceStandardAccount.Account.Money.Amount -= (requiredMoney.Fines + requiredMoney.Main + requiredMoney.Percents);
            credit.PaidFinePart.Amount += requiredMoney.Fines;
            credit.PaidMainPart.Amount += requiredMoney.Main;
            credit.PaidPercentagePart.Amount += requiredMoney.Percents;
            credit.Fine.Amount = credit.PaidFinePart.Amount;
            credit.Percentage.Amount = credit.PaidPercentagePart.Amount;
            credit.Main.Amount = credit.PaidMainPart.Amount;
            _db.Update(credit);
            _db.SaveChanges();
            StandardAccount bankStandardAcc = _creditDb.GetLegalEntities().First().StandardAccounts
                .First(i => i.Account.Money.Currency == credit.Account.Money.Currency);
            bankStandardAcc.Account.Money.Amount += (requiredMoney.Fines + requiredMoney.Main + requiredMoney.Percents);
            _db.Update(bankStandardAcc);
            _db.SaveChanges();

        }

        private void AddFines(CreditAccount credit) 
        {
            credit.Fine.Amount += (credit.CreditTerm.DailyFineRate / 100m) *
                (credit.Account.Money.Amount - credit.PaidMainPart.Amount
                + credit.Fine.Amount - credit.PaidFinePart.Amount
                + credit.Percentage.Amount - credit.PaidPercentagePart.Amount);
            _db.Update(credit);
            _db.SaveChanges();
        }


        public void SkipDay()
        {
            foreach (var credit in _creditDb.GetCreditAccounts().Where(i => i.Account.TerminationDate == null).ToList())
            {
                var paymentCalculator = new CreditPaymentCalculator(credit, _timeService);
                var requiredMoney = paymentCalculator.GetPayment(false);

                if (requiredMoney.Fines > 0 || requiredMoney.Main > 0 || requiredMoney.Percents > 0)
                {
                    if (credit.SourceStandardAccount.Account.Money.Amount >= requiredMoney.Fines + requiredMoney.Main + requiredMoney.Percents)
                    {
                        SubtractMoney(credit, requiredMoney);
                    }
                    else
                    {
                        credit.Main.Amount = requiredMoney.Main  +credit.PaidMainPart.Amount;
                        credit.Percentage.Amount = requiredMoney.Percents + credit.PaidPercentagePart.Amount;
                        credit.Fine.Amount = requiredMoney.Fines + credit.PaidFinePart.Amount;
                        _db.Update(credit);
                        _db.SaveChanges();
                        AddFines(credit);
                    }
                }

                if (paymentCalculator.CheckClosePossibilityByDate(false))
                {
                    Close(credit.Account.Id, true);
                }           
            }
        }
    }
}
