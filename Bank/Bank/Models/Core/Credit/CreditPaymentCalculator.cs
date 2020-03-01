using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class CreditPaymentCalculator
    {
        private readonly CreditAccount _creditAccount;
        private readonly TimeService _timeService;

        private static readonly int _precision = 2;

        public CreditPaymentCalculator(CreditAccount creditAccount, TimeService timeService)
        {
            _creditAccount = creditAccount;
            _timeService = timeService;
        }

        public (decimal Main, decimal Percents, decimal Fines) RequiredToCloseCredit()
        {
            decimal GetPercentsOverall()
            {
                return _creditAccount.CreditTerm.IsAnnuity
                    ? CalculateOverallAnnuityPercentagePayments().Sum(i => i.PercentagePayment)
                    : CalculateOverallDifferencialPercentagePayments().Sum(i => i.PercentagePayment);
            }

            if (!_timeService.CheckTerminationDate(_creditAccount.Account.TerminationDate))
            {
                return (0, 0, 0);
            }

            decimal finesUnpaid = _creditAccount.Fine.Amount - _creditAccount.PaidFinePart.Amount;
            decimal percentsOverallUnpaid = GetPercentsOverall() - _creditAccount.PaidPercentagePart.Amount;
            decimal mainOverallUnpaied = _creditAccount.Account.Money.Amount - _creditAccount.PaidMainPart.Amount;

            return (Main: mainOverallUnpaied, Percents: percentsOverallUnpaid, Fines: finesUnpaid);
        }

        private List<(decimal MainMoneyLeft, int DaysLeft)> GetListOfLeftMoneyAndDays()
        {
            decimal currentMainLeft = _creditAccount.Account.Money.Amount;
            int daysLeft = (int)_creditAccount.CreditTerm.InterestAccrual.TermInDays;
            var mainLeft = new List<(decimal MainMoneyLeft, int DaysLeft)>();
            while (currentMainLeft > 0 && daysLeft > 0)
            {
                mainLeft.Add((currentMainLeft, daysLeft));
                currentMainLeft -= Math.Round((decimal)TimeService.DaysInMonth / (int)_creditAccount.CreditTerm.InterestAccrual.TermInDays
                    * _creditAccount.Account.Money.Amount, _precision);
                daysLeft -= TimeService.DaysInMonth;
            }
            //if (currentMainLeft > 0)
            //{
            //    mainLeft[mainLeft.Count - 1] = 
            //        (MainMoneyLeft: mainLeft[mainLeft.Count - 1].MainMoneyLeft + currentMainLeft,
            //        DaysLeft: mainLeft[mainLeft.Count - 1].DaysLeft);
            //}

            return mainLeft;
        }

        private List<int> SplitTermOnMonths(int term)
        {
            var result = Enumerable.Repeat(TimeService.DaysInMonth, term / TimeService.DaysInMonth)
                .Concat(term % TimeService.DaysInMonth > 0 ? new[] { term % TimeService.DaysInMonth } : new int[] { })
                .ToList();

            return result;
        }

        private List<(decimal MainMoneyLeft, int SegmentLengthInDays, decimal PercentagePayment)> CalculateOverallDifferencialPercentagePayments()
        {
            List<(decimal MainMoneyLeft, int SegmentLengthInDays)> ConvertToSegments(List<(decimal MainMoneyLeft, int DaysLeft)> left)
            {
                var result = new List<(decimal MainMoneyLeft, int SegmentLengthInDays)>();
                var terms = SplitTermOnMonths(left.FirstOrDefault().DaysLeft);

                for (int i = 0; i < left.Count; i++)
                {
                    result.Add((MainMoneyLeft: left[i].MainMoneyLeft, SegmentLengthInDays: terms[i]));
                }
                return result;
            }

            return ConvertToSegments(GetListOfLeftMoneyAndDays()).Select(i =>
                (MainMoneyLeft: i.MainMoneyLeft,
                SegmentLengthInDays: i.SegmentLengthInDays,
                PercentagePayment: Math.Round(i.MainMoneyLeft
                    * (i.SegmentLengthInDays / (decimal)TimeService.DaysInYear)
                    * (_creditAccount.CreditTerm.InterestRate / 100.0m), _precision)))
                .ToList();
        }

        private List<(int SegmentLengthInDays, decimal PercentagePayment)> CalculateOverallAnnuityPercentagePayments()
        {
            return SplitTermOnMonths((int)_creditAccount.CreditTerm.InterestAccrual.TermInDays).Select(i =>
                (SegmentLengthInDays: i,
                PercentagePayment: Math.Round(i / (decimal)TimeService.DaysInYear
                    * (_creditAccount.CreditTerm.InterestRate / 100.0m)
                    * _creditAccount.Account.Money.Amount, _precision)))
                .ToList();
        }

        private decimal CalculatePercentsForSegment(int segmentLength, decimal mainMoneyLeftForDifferencial = 0)
        {
            return _creditAccount.CreditTerm.IsAnnuity
                 ? CalculateOverallAnnuityPercentagePayments().FirstOrDefault(i => i.SegmentLengthInDays == segmentLength).PercentagePayment
                 : CalculateOverallDifferencialPercentagePayments().FirstOrDefault(i => i.MainMoneyLeft == mainMoneyLeftForDifferencial).PercentagePayment;
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
            (decimal MainUnpaid, decimal PercentsUnpaid, decimal FinesUnpaid) GetActualUnpaid()
            {
                decimal finesUnpaid = _creditAccount.Fine.Amount - _creditAccount.PaidFinePart.Amount;
                decimal percentsUnpaid = _creditAccount.Percentage.Amount - _creditAccount.PaidPercentagePart.Amount;
                decimal mainUnpaid = _creditAccount.Main.Amount - _creditAccount.PaidMainPart.Amount;

                return (MainUnpaid: mainUnpaid, PercentsUnpaid: percentsUnpaid, FinesUnpaid: finesUnpaid);
            }

            // no logic calulations
            static (decimal Main, decimal Percents, decimal Fines) GetPaymentByLeftPartAndUnpaidPart(
                (decimal MainUnpaid, decimal PercentsUnpaid, decimal FinesUnpaid) unpaidNow,
                decimal mainPart, decimal percentagePart)
            {
                return (Main: unpaidNow.MainUnpaid + mainPart,
                    Percents: unpaidNow.PercentsUnpaid + percentagePart,
                    Fines: unpaidNow.FinesUnpaid);
            }

            if (_timeService.IsActive(_creditAccount.Account.OpenDate, _creditAccount.Account.TerminationDate))
            {
                var unpaidNow = GetActualUnpaid();

                if (_timeService.CountElapsedDays(_creditAccount.Account.OpenDate) + GetPredictionShift(prediction)
                    == _creditAccount.CreditTerm.InterestAccrual.TermInDays)
                {
                    var leftPart = GetListOfLeftMoneyAndDays().Last();
                    return GetPaymentByLeftPartAndUnpaidPart(
                        unpaidNow: unpaidNow,
                        mainPart: leftPart.MainMoneyLeft,
                        percentagePart: CalculatePercentsForSegment(
                            leftPart.DaysLeft % TimeService.DaysInMonth == 0 ? TimeService.DaysInMonth : leftPart.DaysLeft % TimeService.DaysInMonth,
                            leftPart.MainMoneyLeft));
                }
                else if (_timeService.IsMultipleOfMonth(_creditAccount.Account.OpenDate.AddDays(-GetPredictionShift(prediction))))
                {
                    var listOfAllPaymentsAndDays = GetListOfLeftMoneyAndDays();
                    (decimal MainMoneyLeft, int DaysLeft) leftPart;
                   
                    leftPart = listOfAllPaymentsAndDays.FirstOrDefault(i => i.MainMoneyLeft == _creditAccount.Account.Money.Amount - _creditAccount.PaidMainPart.Amount);

                    int index = listOfAllPaymentsAndDays.FindIndex(i => i == leftPart);
                    decimal mainPart = index == listOfAllPaymentsAndDays.Count - 1
                        ? listOfAllPaymentsAndDays.Last().MainMoneyLeft
                        : listOfAllPaymentsAndDays.ElementAt(index).MainMoneyLeft - listOfAllPaymentsAndDays.ElementAt(index + 1).MainMoneyLeft;

                    return GetPaymentByLeftPartAndUnpaidPart(
                        unpaidNow: unpaidNow,
                        mainPart: mainPart,
                        percentagePart: CalculatePercentsForSegment(
                            leftPart.DaysLeft % TimeService.DaysInMonth == 0 ? TimeService.DaysInMonth : leftPart.DaysLeft % TimeService.DaysInMonth,
                            leftPart.MainMoneyLeft));
                }
                else
                {
                    return (Main: unpaidNow.MainUnpaid, Percents: unpaidNow.PercentsUnpaid, Fines: unpaidNow.FinesUnpaid);
                }
            }
            return (0, 0, 0);
        }
    }
}
