using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Describes deposits variable terms
    /// </summary>
    public class DepositVariable
    {
        public int Id { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public List<DepositCore> DepositCores { get; set; }

        public int DepositGeneralId { get; set; }

        public DepositGeneral DepositGeneral { get; set; }

        public int MinimalDepositId { get; set; }
        public Money MinimalDeposit { get; set; }
    }
}
