using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Step 1
    /// </summary>
    public class SelectCurrencyViewModel
    {
        [DisplayName("Currency")]
        public int CurrencyId { get; set; } = 1;

        [DisplayName("Currency")]
        public List<Currency> CurrencyList { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }
    }
}
