using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{

    public class Money
    {
        public int Id { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public Account Account { get; set; }

        public DepositVariable DepositVariable { get; set; }

        public DepositAccount DepositAccount { get; set; }

        public CreditAccount CreditAccountPaidMainPart { get; set; }

        public CreditAccount CreditAccountPaidPercentagePart { get; set; }

        public CreditAccount CreditAccountPaidFinePart { get; set; }

        public CreditAccount CreditAccountFine { get; set; }
    
        public CreditAccount CreditAccountPercentage { get; set; }

        public CreditAccount CreditAccountMain { get; set; }

        public CreditTerm CreditTermMinimalCredit { get; set; }

        public CreditTerm CreditTermMaximalCredit { get; set; }

        public Transaction Transaction { get; set; }
    }
}
