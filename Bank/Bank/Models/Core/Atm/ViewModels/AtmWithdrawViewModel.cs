using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class AtmWithdrawViewModel
    {
        [DisplayName("Account")]
        public int AccountId { get; set; }

        [DisplayName("Money")]
        public decimal Money { get; set; } 

        [DisplayName("Enter amount")]
        public decimal AmountToWithdraw { get; set; }

        [RegularExpression(@"^([0-9])+$", ErrorMessage = "PIN code should contain digits only.")]
        [StringLength(4)]
        [DisplayName("PIN")]
        [PasswordPropertyText]
        public string PinCode { get; set; } = "";
    }
}
