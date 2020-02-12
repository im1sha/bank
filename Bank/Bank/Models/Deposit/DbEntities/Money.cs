using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models 
{
    
    public class Money
    {
        public int Id { get; set; }

        public Currency Currency { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        #region should have 1 only referenced, others == null
        public int? AccountId { get; set; }
        public Account Account { get; set; }

        public int? DepositVariableId { get; set; }
        public DepositVariable DepositVariable { get; set; }

        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        #endregion
    }
}
