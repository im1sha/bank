using System.Collections.Generic;

namespace Bank.Models
{
    /// <summary>
    /// Interest accrual term
    /// </summary>
    public class InterestAccrual
    {
        public int Id { get; set; }

        /// <summary>
        /// Month, Year, 30 days, 370 days, 195 days etc.
        /// </summary>
        public string Name { get; set; }

        public int? TermInDays { get; set; }

        public List<DepositCore> DepositCores { get; set; }

        //public List<CreditCore> CreditCores { get; set; }
    }
}

