using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Transaction items
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public Account AnotherAccount { get; set; }

        public bool IsRecipient { get; set; }

        public int AmountId { get; set; }

        public Money Amount { get; set; }
    }
}
