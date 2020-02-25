namespace Bank.Models
{
    public class CreditAccount
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public Account Account { get; set; }

        public Money Profit { get; set; }

        public int DepositCoreId { get; set; }

        public DepositCore DepositCore { get; set; }
    }
}
