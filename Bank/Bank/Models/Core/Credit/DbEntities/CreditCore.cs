using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Deposit variable has m2m relation with InterestAccrual
    /// </summary>
    public class CreditCore
    {
        public int DepositVariableId { get; set; }
        public DepositVariable DepositVariable { get; set; }
        public int InterestAccrualId { get; set; }
        public InterestAccrual InterestAccrual { get; set; }

        /// <summary>
        /// % 
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InterestRate { get; set; }

        public List<DepositAccount> DepositAccounts { get; set; }
    }
}
