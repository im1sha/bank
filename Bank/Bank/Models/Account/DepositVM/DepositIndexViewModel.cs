using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class DepositIndexViewModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Owner { get; set; }

        [DisplayName("Passport")]
        public string Passport { get; set; }

        [DisplayName("Name")]
        public string AccountName { get; set; }

        [DisplayName("Deposit")]
        public string DepositName { get; set; }

        public string Currency { get; set; }

        [DisplayName("Amount")]
        public decimal MoneyAmount { get; set; }

        public decimal Profit { get; set; }

        [DisplayName("Active")]
        public string IsActive { get; set; }

        [DisplayName("Interest rate")]
        public decimal InterestRate { get; set; }

        public int AccountId { get; set; }

        [DisplayName("Account")]
        public string AccountNumber { get; set; }

        [DisplayName("Open")]
        public DateTime OpenDate { get; set; }

        [DisplayName("Terminated")]
        public DateTime? TerminationDate { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }           

        [DisplayName("Is revocable")]
        public string IsRevocable { get; set; }

        [DisplayName("With capitalization")]
        public string WithCapitalization { get; set; }

        [DisplayName("Replenishment allowed")]
        public string ReplenishmentAllowed { get; set; }     
    }
}
