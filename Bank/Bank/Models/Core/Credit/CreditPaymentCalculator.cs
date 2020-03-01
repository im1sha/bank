using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class CreditPaymentCalculator
    {
        private readonly CreditAccount _creditAccount;
        private readonly TimeService _timeService;

        public CreditPaymentCalculator(CreditAccount creditAccount, TimeService timeService)
        {
            _creditAccount = creditAccount;
            _timeService = timeService;
        }

        public (decimal Main, decimal Percents, decimal Fines) RequiredToCloseCreditPrediction()
        {
            if (!_timeService.CheckTerminationDate(_creditAccount.Account.TerminationDate))
            {
                return (0, 0, 0);
            }

            decimal finesUnpaid = _creditAccount.Fine.Amount - _creditAccount.PaidFinePart.Amount;

            decimal percentsOverallUnpaid =
                (_creditAccount.CreditTerm.IsAnnuity
                    ? ((int)_creditAccount.CreditTerm.InterestAccrual.TermInDays / (decimal)TimeService.DaysInYear)
                        * (_creditAccount.CreditTerm.InterestRate / 100.0m)
                        * _creditAccount.Account.Money.Amount
                    : CalculateOverallDifferencialPercentagePayments()
                )
                - _creditAccount.PaidPercentagePart.Amount;

            decimal mainOverallUnpaied = _creditAccount.Account.Money.Amount - _creditAccount.PaidMainPart.Amount;

            return (Main: mainOverallUnpaied, Percents: percentsOverallUnpaid, Fines: finesUnpaid);
        }

        public bool CheckClosePossibilityByDate(bool prediction)
        {
            return _timeService.CountElapsedDays(_creditAccount.Account.OpenDate) + GetPredictionShift(prediction)
                >= _creditAccount.CreditTerm.InterestAccrual.TermInDays
                && _creditAccount.Account.TerminationDate == null;
        }

        private static int GetPredictionShift(bool prediction)
        {
            return prediction ? 1 : 0;
        }

        public (decimal Main, decimal Percents, decimal Fines) GetPayment(bool prediction = true)
        {
            (decimal Main, decimal Percents, decimal Fines) GetPaymentByLeftPartAndUnpaidPart(
                (decimal MainUnpaid, decimal PercentsUnpaid, decimal FinesUnpaid) unpaidNow,
                (decimal AmountToCount, decimal Period) leftPart, decimal mainPart)
            {
                return (Main: unpaidNow.MainUnpaid + mainPart,
                    Percents: (leftPart.AmountToCount * (leftPart.Period / TimeService.DaysInYear) * (_creditAccount.CreditTerm.InterestRate / 100.0m)) + unpaidNow.PercentsUnpaid,
                    Fines: unpaidNow.FinesUnpaid);
            }

            if (_timeService.IsActive(_creditAccount.Account.OpenDate, _creditAccount.Account.TerminationDate))
            {
                var unpaidNow = GetActualUnpaid();

                if (_timeService.IsMultipleOfMonth(_creditAccount.Account.OpenDate.AddDays(-GetPredictionShift(prediction))))
                {
                    var lists = GetListOfLeftMoneyAndDays();
                    var index = ((_timeService.CountElapsedDays(_creditAccount.Account.OpenDate) + GetPredictionShift(prediction))
                        / TimeService.DaysInMonth) - 1;

                    (decimal MainMoneyLeft, decimal DaysLeft) left;

                    if (lists.Count <= index)
                    {
                        left = lists.Last();
                    }
                    else
                    {
                        left = lists.ElementAt(index);
                    }

                    var mainPart = (decimal)TimeService.DaysInMonth / (int)_creditAccount.CreditTerm.InterestAccrual.TermInDays
                        * _creditAccount.Account.Money.Amount;

                    return GetPaymentByLeftPartAndUnpaidPart(unpaidNow,
                        (AmountToCount: _creditAccount.CreditTerm.IsAnnuity ? _creditAccount.Account.Money.Amount : left.MainMoneyLeft,
                        Period: TimeService.DaysInMonth), mainPart: mainPart);
                }
                else if (_timeService.CountElapsedDays(_creditAccount.Account.OpenDate) + GetPredictionShift(prediction)
                    == _creditAccount.CreditTerm.InterestAccrual.TermInDays)
                {
                    var leftPart = GetListOfLeftMoneyAndDays().Last();

                    return GetPaymentByLeftPartAndUnpaidPart(unpaidNow,
                        (AmountToCount: _creditAccount.CreditTerm.IsAnnuity ? _creditAccount.Account.Money.Amount : leftPart.MainMoneyLeft,
                        Period: leftPart.DaysLeft), mainPart: leftPart.MainMoneyLeft);
                }
                else
                {
                    return (Main: unpaidNow.MainUnpaid, Percents: unpaidNow.PercentsUnpaid, Fines: unpaidNow.FinesUnpaid);
                }
            }
            return (0, 0, 0);
        }

        private (decimal MainUnpaid, decimal PercentsUnpaid, decimal FinesUnpaid) GetActualUnpaid()
        {
            decimal finesUnpaid = _creditAccount.Fine.Amount - _creditAccount.PaidFinePart.Amount;
            decimal percentsUnpaid = _creditAccount.Percentage.Amount - _creditAccount.PaidPercentagePart.Amount;
            decimal mainUnpaid = _creditAccount.Main.Amount - _creditAccount.PaidMainPart.Amount;

            return (MainUnpaid: mainUnpaid, PercentsUnpaid: percentsUnpaid, FinesUnpaid: finesUnpaid);
        }

        private List<(decimal MainMoneyLeft, decimal DaysLeft)> GetListOfLeftMoneyAndDays()
        {
            decimal currentMainLeft = _creditAccount.Account.Money.Amount;
            decimal daysLeft = (int)_creditAccount.CreditTerm.InterestAccrual.TermInDays;
            var mainLeft = new List<(decimal MainMoneyLeft, decimal DaysLeft)>();
            while (currentMainLeft > 0)
            {
                mainLeft.Add((currentMainLeft, daysLeft));
                currentMainLeft -= (decimal)TimeService.DaysInMonth / (int)_creditAccount.CreditTerm.InterestAccrual.TermInDays
                    * _creditAccount.Account.Money.Amount;
                daysLeft -= TimeService.DaysInMonth;
            }

            return mainLeft;
        }

        private decimal CalculateOverallDifferencialPercentagePayments()
        {
            var mainLeft = GetListOfLeftMoneyAndDays();
            return mainLeft.Sum(i =>
                i.MainMoneyLeft
                * ((i.DaysLeft > TimeService.DaysInMonth ? TimeService.DaysInMonth : i.DaysLeft) / TimeService.DaysInYear)
                * (_creditAccount.CreditTerm.InterestRate / 100.0m));
        }
    }
}
