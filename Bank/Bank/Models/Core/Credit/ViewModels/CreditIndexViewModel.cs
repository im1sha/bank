using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class CreditIndexViewModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Owner { get; set; }

        [DisplayName("Passport")]
        public string Passport { get; set; }

        [DisplayName("Name")]
        public string AccountName { get; set; }

        [DisplayName("Credit")]
        public string CreditName { get; set; }

        public string Currency { get; set; }

        [DisplayName("Amount")]
        public decimal MoneyAmount { get; set; }

        [DisplayName("Percentage amount")]
        public decimal Percentage { get; set; }

        [DisplayName("Fines")]
        public decimal Fine { get; set; }

        [DisplayName("Paid main part")]
        public decimal PaidMainPart { get; set; }

        [DisplayName("Paid percentage")]
        public decimal PaidPercentagePart { get; set; }
       
        [DisplayName("Paid fines")]
        public decimal PaidFinePart { get; set; }

        [DisplayName("Next payment")]
        public decimal NextPayment { get; set; }

        [DisplayName("Left")]
        public decimal RequiredToCloseCredit { get; set; }
      
        [DisplayName("Active")]
        public string IsActive { get; set; }

        [DisplayName("Interest rate")]
        public decimal InterestRate { get; set; }

        public int AccountId { get; set; }

        [DisplayName("Account")]
        public string AccountNumber { get; set; }

        [DisplayName("Open")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }

        [DisplayName("Terminated")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }

        [DisplayName("Minimal credit amount")]
        public decimal MinimalCredit { get; set; }

        [DisplayName("Maximal credit amount")]
        public decimal MaximalCredit { get; set; }

        [DisplayName("Early repayment allowed")]
        public string EarlyRepaymentAllowed { get; set; }

        [DisplayName("Annuity form")]
        public string IsAnnuity { get; set; }

        [DisplayName("Daily fine rate")]
        public decimal DailyFineRate { get; set; }    
    }
}
