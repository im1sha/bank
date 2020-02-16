using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class StandardAccountCreateViewModel
    {
        public int Id { get; set; }

        [DisplayName("Account number")]
        public string Number { get; set; }

        public string Name { get; set; }

        [Required]
        [DisplayName("Currency")]
        public int CurrencyId { get; set; } = 1;

        [DisplayName("Currency")]
        public List<Currency> CurrencyList { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [Currency(true)]
        public string Amount { get; set; }

        public bool IsPerson { get; set; }
        public int OwnerId { get; set; }

        public string Owner { get; set; }
    }
}
