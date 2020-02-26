using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class CreditCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Credit name")]
        public string Name { get; set; }

        [DisplayName("Account")]
        public string CreditNumber { get; set; }

        #region const : person data

        public int OwnerId { get; set; }

        [DisplayName("Owner")]
        public string Owner { get; set; }

        [DisplayName("Passport")]
        public string Passport { get; set; }

        #endregion

        [DisplayName("Source account")]
        public int AccountSourceId { get; set; }

        [DisplayName("Source account")]
        public List<Account> AccountSourceList { get; set; }

        [DisplayName("Source account")]
        public string AccountSourceName { get; set; }

        [DisplayName("Source account money")]
        public decimal MoneyAmount { get; set; }


        [DisplayName("Currency")]
        public int CurrencyId { get; set; }

        [DisplayName("Currency")]
        public List<Currency> CurrencyList { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }


        [DisplayName("Credit")]
        public int CreditTermId { get; set; }

        [DisplayName("Credit")]
        public List<CreditTerm> CreditTermList { get; set; }

        [DisplayName("Credit")]
        public string CreditTermName { get; set; }


        [DisplayName("Interest rate")]
        public decimal InterestRate { get; set; }

        [DisplayName("Term")]
        public int InterestAccrualId { get; set; }

        [DisplayName("Term")]
        public List<InterestAccrual> InterestAccrualList { get; set; }

        [DisplayName("Term")]
        public string TermName { get; set; }


        [DisplayName("Open")]
        [DataType(DataType.Date)]
        //[AgeDateRange(0, 0, 1, 0)]
        public DateTime OpenDate { get; set; }

        [DisplayName("Terminated")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [DisplayName("Early repayment allowed")]
        public string EarlyRepaymentAllowed { get; set; }

        [DisplayName("Annuity form")]
        public string IsAnnuity { get; set; }

        [DisplayName("Daily fine rate")]
        public decimal DailyFineRate { get; set; }

        [DisplayName("Minimal credit amount")]
        public decimal MinimalCredit { get; set; }

        [DisplayName("Maximal credit amount")]
        public decimal MaximalCredit { get; set; }

        [DisplayName("Selected credit amount")]
        public decimal SelectedCredit { get; set; }       
    }
}

