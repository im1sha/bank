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

        //#region should have 1 only referenced, others == null

        public Account Account { get; set; }

        public DepositVariable DepositVariable { get; set; }
        public DepositAccount DepositAccount { get; set; }

        //public CreditVariable CreditVariable { get; set; }
        //public CreditAccount CreditAccount { get; set; }

        public Transaction Transaction { get; set; }

        //#endregion
    }
}
