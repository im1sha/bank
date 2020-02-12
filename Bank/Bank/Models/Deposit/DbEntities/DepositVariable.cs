﻿using System.Collections.Generic;

namespace Bank.Models
{
    /// <summary>
    /// Describes deposits variable terms
    /// </summary>
    public class DepositVariable
    {
        public int Id { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        /// <summary>
        /// % 
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Term in days
        /// </summary>
        public int Duration { get; set; }

        public int DepositGeneralId { get; set; }

        public DepositGeneral DepositGeneral { get; set; }

        public List<DepositAccount> DepositAccounts { get; set; }
    }
}
