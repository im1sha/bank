namespace Bank.Models
{
    public class DepositAccount
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public Account Account { get; set; }

        public int DepositVariableId { get; set; }

        public DepositVariable DepositVariable { get; set; }
    }
}
