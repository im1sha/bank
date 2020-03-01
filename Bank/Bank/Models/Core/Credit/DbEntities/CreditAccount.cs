using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class CreditAccount
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public Account Account { get; set; }

        public int SourceStandardAccountId { get; set; }
        public StandardAccount SourceStandardAccount { get; set; }

        public int MainId { get; set; }

        public Money Main{ get; set; }

        public int PaidMainPartId { get; set; }

        public Money PaidMainPart { get; set; }

        public int PercentageId { get; set; }

        public Money Percentage { get; set; }

        public int PaidPercentagePartId { get; set; }

        public Money PaidPercentagePart { get; set; }

        public int PaidFinePartId { get; set; }

        public Money PaidFinePart { get; set; }

        public int FineId { get; set; }

        public Money Fine { get; set; }

        public int CreditTermId { get; set; }

        public CreditTerm CreditTerm { get; set; }
    }
}
