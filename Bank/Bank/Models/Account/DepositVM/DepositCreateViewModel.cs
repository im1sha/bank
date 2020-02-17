using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class DepositCreateViewModel
    {
        





        [DisplayName("Currency")]
        public int CurrencyId { get; set; }

        [DisplayName("Currency")]
        public List<Currency> CurrencyList { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }


        [DisplayName("Deposit")]
        public int DepositGeneralId { get; set; }

        [DisplayName("Deposit")]
        public List<DepositGeneral> DepositGeneralList { get; set; }

        [DisplayName("Deposit")]
        public string DepositName { get; set; }


        [DisplayName("Revocable")]
        public string IsRevocable { get; set; }

        [DisplayName("With capitalization")]
        public string WithCapitalization { get; set; }

        [DisplayName("Replenishment allowed")]
        public string ReplenishmentAllowed { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start date")]
        [AgeDateRange(0, 0, 1, 0)]
        public DateTime? StartDate { get; set; } = DateTime.Now;


        [Required]
        [DisplayName("Term")]
        public int InterestAccrualId { get; set; } 

        [Required]
        [DisplayName("Term")]
        public List<InterestAccrual> InterestAccrualList { get; set; }
        
        [DisplayName("Term")]
        public string Term { get; set; }


        [DisplayName("Interest rate")]
        public string InterestRate { get; set; }

        [DisplayName("Amount of money")]
        public decimal TotalMoney { get; set; }

        [DisplayName("Required amount of money")]
        public decimal RequiredMoney { get; set; }

        [DisplayName("Selected amount of money")]
        public decimal SelectedMoney { get; set; }
    }
}
