//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

//namespace Bank.Models
//{
//    public class CreditCreateViewModel
//    {
//        public int Id { get; set; }

//        [Required]
//        [DisplayName("Deposit name")]
//        public string Name { get; set; }

//        [DisplayName("Account")]
//        public string DepositNumber { get; set; }

//        #region const : person data

//        public int OwnerId { get; set; }

//        [DisplayName("Owner")]
//        public string Owner { get; set; }

//        [DisplayName("Passport")]
//        public string Passport { get; set; }

//        #endregion

//        [DisplayName("Source account")]
//        public int AccountSourceId { get; set; }

//        [DisplayName("Source account")]
//        public List<AccountViewModel> AccountSourceList { get; set; }

//        [DisplayName("Source account")]
//        public string AccountSourceName { get; set; }

//        [DisplayName("Source account money")]
//        public decimal MoneyAmount { get; set; }


//        [DisplayName("Currency")]
//        public int CurrencyId { get; set; }

//        [DisplayName("Currency")]
//        public List<CurrencyViewModel> CurrencyList { get; set; }

//        [DisplayName("Currency")]
//        public string CurrencyName { get; set; }


//        [DisplayName("Deposit")]
//        public int DepositGeneralId { get; set; }

//        [DisplayName("Deposit")]
//        public List<DepositGeneralViewModel> DepositGeneralList { get; set; }

//        [DisplayName("Deposit")]
//        public string DepositName { get; set; }


//        [DisplayName("Interest rate")]
//        public decimal InterestRate { get; set; }

//        [DisplayName("Term")]
//        public int InterestAccrualId { get; set; }

//        [DisplayName("Term")]
//        public List<InterestAccrualViewModel> InterestAccrualList { get; set; }

//        [DisplayName("Term")]
//        public string TermName { get; set; }


//        [DisplayName("Open")]
//        [DataType(DataType.Date)]
//        //[AgeDateRange(0, 0, 1, 0)]
//        public DateTime OpenDate { get; set; }

//        [DisplayName("Terminated")]
//        [DataType(DataType.Date)]
//        public DateTime? TerminationDate { get; set; }


//        [DisplayName("Is revocable")]
//        public string IsRevocable { get; set; }

//        [DisplayName("With capitalization")]
//        public string WithCapitalization { get; set; }

//        [DisplayName("Replenishment allowed")]
//        public string ReplenishmentAllowed { get; set; }


//        [DisplayName("Required amount of money")]
//        public decimal RequiredMoney { get; set; }

//        [DisplayName("Selected amount of money")]
//        public decimal SelectedMoney { get; set; }
//    }
//}

