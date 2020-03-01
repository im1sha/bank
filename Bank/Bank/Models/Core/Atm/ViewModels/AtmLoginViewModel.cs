using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class AtmLoginViewModel
    {
        [DisplayName("Account")]
        public int AccountId { get; set; } = 1;

        [DisplayName("Account")]
        public List<Account> AccountList { get; set; }

        [RegularExpression(@"^([0-9])+$", ErrorMessage = "PIN code should contain digits only.")]
        [StringLength(4)]
        [DisplayName("PIN")]
        [PasswordPropertyText]
        public string PinCode { get; set; } = "";
    }
}
