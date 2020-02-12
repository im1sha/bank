using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Step 5
    /// </summary>
    public class SelectDepositSummaryViewModel
    {
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Deposit")]
        public string DepositName { get; set; }

        [DisplayName("Interest rate")]
        public string InterestRate { get; set; }

        [DisplayName("Is revocable")]
        public string IsRevocable { get; set; }

        [DisplayName("With capitalization")]
        public string WithCapitalization { get; set; }

        [DisplayName("Replenishment allowed")]
        public string ReplenishmentAllowed { get; set; }

        [DisplayName("Start date")]
        public string StartDate { get; set; }

        [DisplayName("End date")]
        public string EndDate { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }

        [DisplayName("Selected amount of money")]
        public string SelectedMoney { get; set; }
    }
}
