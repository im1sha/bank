using System.Collections.Generic;

namespace Bank.Models
{
    public class InterestAccrual
    {
        public int Id { get; set; }

        /// <summary>
        /// Month, Year, 30 days, 370 days, 195 days etc.
        /// </summary>
        public string Term { get; set; }

        public List<DepositGeneral> DepositGenerals { get; set; }
    }
}