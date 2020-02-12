using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Step 4
    /// </summary>
    public class SelectAmountViewModel
    {
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Deposit")]
        public string DepositName { get; set; }

        [DisplayName("Interest rate")]
        public string InterestRate { get; set; }

        [DisplayName("Start date")]
        public string StartDate { get; set; }

        [DisplayName("End date")]
        public string EndDate { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }

        [DisplayName("Amount of money")]
        public decimal TotalMoney { get; set; }

        [DisplayName("Required amount of money")]
        public decimal RequiredMoney { get; set; }

        [DisplayName("Selected amount of money")]
        public decimal SelectedMoney { get; set; }
    }
}
