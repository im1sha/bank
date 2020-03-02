using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class AtmCellularInputViewModel
    {
        public string Cellular { get; set; }

        [DisplayName("Number")]
        [RegularExpression(@"^([0-9])+$", ErrorMessage = "Number should contain digits only.")]
        [StringLength(9)]
        public string Number { get; set; }
    }
}
