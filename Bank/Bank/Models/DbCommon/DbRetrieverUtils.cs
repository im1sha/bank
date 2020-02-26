using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public static class DbRetrieverUtils
    {
        public static string GenerateNewStandardAccountId(DepositDbEntityRetriever depositDbEntityRetriever)
        {
            const int otherPartLength = 9;
            var standardAccountDefaultPart = "9999";
            var standardAccountDefaultPartLength = 4;

            return standardAccountDefaultPart
                + (depositDbEntityRetriever.GetAccounts()
                .Select(i => decimal.Parse(string.Join("", i.Number.TakeLast(i.Number.Count() - standardAccountDefaultPartLength))))
                .Aggregate((val, i) => Math.Max(i, val)) + 1).ToString().PadLeft(otherPartLength, '0');
        }

        public static string GenerateNewDepositId(DepositDbEntityRetriever depositDbEntityRetriever)
        {
            const int otherPartLength = 9;
            var depositAccountDefaultPart = "3014";
            var depositAccountDefaultPartLength = 4;

            return depositAccountDefaultPart
                + (depositDbEntityRetriever.GetAccounts()
                .Select(i => decimal.Parse(string.Join("", i.Number.TakeLast(i.Number.Count() - depositAccountDefaultPartLength))))
                .Aggregate((val, i) => Math.Max(i, val)) + 1).ToString().PadLeft(otherPartLength, '0');
        }

        internal static string GenerateNewCreditId(CreditDbEntityRetriever creditDbEntityRetriever)
        {
            const int otherPartLength = 9;
            var creditAccountDefaultPart = "1111";
            var creditAccountDefaultPartLength = 4;

            return creditAccountDefaultPart
                + (creditDbEntityRetriever.GetAccounts()
                .Select(i => decimal.Parse(string.Join("", i.Number.TakeLast(i.Number.Count() - creditAccountDefaultPartLength))))
                .Aggregate((val, i) => Math.Max(i, val)) + 1).ToString().PadLeft(otherPartLength, '0');
        }
    }
    
}
