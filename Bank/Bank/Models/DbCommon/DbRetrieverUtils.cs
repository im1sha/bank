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
            var standardAccountDefaultPart = "3014";
            var standardAccountDefaultPartLength = 4;

            return standardAccountDefaultPart
                + (depositDbEntityRetriever.GetAccounts()
                .Select(i => decimal.Parse(string.Join("", i.Number.TakeLast(i.Number.Count() - standardAccountDefaultPartLength))))
                .Aggregate((val, i) => Math.Max(i, val)) + 1).ToString().PadLeft(otherPartLength, '0');
        }
    }
}
