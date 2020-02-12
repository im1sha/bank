namespace Bank.Models
{
    public class Log
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public bool IsRecipient { get; set; }

        public int AnotherAccountId { get; set; }

        public Account AnotherAccount { get; set; }

        public decimal TransactionFund { get; set; }
    }
}
