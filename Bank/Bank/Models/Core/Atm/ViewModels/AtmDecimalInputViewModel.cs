using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class AtmDecimalInputViewModel
    {
        public string Action { get; set; }

        [DisplayName("Enter amount")]      
        [Currency(true)]
        [Required]
        public string Amount { get; set; }
    }
}
