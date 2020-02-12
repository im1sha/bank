using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Step 3
    /// </summary>
    public class SelectTermViewModel
    {
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Deposit")]
        public string DepositName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start date")]
        [AgeDateRange(0, 0, 1, 0)]
        public DateTime? StartDate { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Term")]
        public int InterestAccrualId { get; set; } = 1;

        [Required]
        [DisplayName("Term")]
        public List<(InterestAccrual InterestAccrual, string InterestRate)> InterestAccrualList { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }

        [DisplayName("Interest rate")]
        public string InterestRate { get; set; }
    }
}
