using System.Collections.Generic;

namespace Bank.Models
{
    /// <summary>
    /// Describes deposits terms
    /// </summary>
    public class DepositGeneral
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Whether it's possible withdraw all the deposit
        /// before deposit has expired
        /// </summary>
        public bool IsRevocable { get; set; }

        public bool WithCapitalization { get; set; }

        public decimal MinimalDeposit { get; set; }

        /// <summary>
        /// Can bring additional bankroll(cash) to deposit 
        /// </summary>
        public bool ReplenishmentAllowed { get; set; }

        public int InterestAccrualId { get; set; }

        /// <summary>
        /// Term in which percents will been accrued
        /// </summary>
        public InterestAccrual InterestAccrual { get; set; }

        public List<DepositVariable> DepositVariables { get; set; }
    }
}
