using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class CreditTerm
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public int InterestAccrualId { get; set; }

        public InterestAccrual InterestAccrual { get; set; }

        public int MinimalCreditId { get; set; }
        public Money MinimalCredit { get; set; }

        public int MaximalCreditId { get; set; }
        public Money MaximalCredit { get; set; }

        public bool EarlyRepaymentAllowed { get; set; }

        public bool IsAnnuity { get; set; }

        /// <summary>
        /// Fines calculated relatively 
        /// to unpaid fines + unpaid percents 
        /// + unpaid amount of main part
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DailyFineRate { get; set; }

        /// <summary>
        /// % 
        /// </summary>
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InterestRate { get; set; }

        public List<CreditAccount> CreditAccounts { get; set; }
    }
}
