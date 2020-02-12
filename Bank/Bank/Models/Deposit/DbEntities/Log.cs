using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Transaction items
    /// </summary>
    public class Log
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public bool IsRecipient { get; set; }

        public int AnotherAccountId { get; set; }

        public Account AnotherAccount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransactionFund { get; set; }
    }
}
