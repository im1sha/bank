using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Step 2
    /// </summary>
    public class SelectDepositViewModel
    {
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Deposit")]
        public int DepositGeneralId { get; set; } = 1;

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
    }
}
