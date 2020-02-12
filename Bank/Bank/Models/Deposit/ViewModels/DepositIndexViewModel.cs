using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class DepositIndexViewModel
    {
        public int AccountId { get; set; }

        public int PersonId { get; set; }

        [DisplayName("Account number")]
        public string AccountNumber { get; set; }

        [DisplayName("Currency name")]
        public string CurrencyName { get; set; }

        [DisplayName("Active")]
        public string IsActive { get; set; }

        [DisplayName("Name")]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        public string LastName { get; set; }

        public string Deposit { get; set; }
    }
}
