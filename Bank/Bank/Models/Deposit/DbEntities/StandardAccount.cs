namespace Bank.Models
{
    /// <summary>
    /// Stores financials
    /// </summary>
    public class StandardAccount
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
