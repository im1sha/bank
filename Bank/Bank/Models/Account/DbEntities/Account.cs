using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Plan of account(план учета) : list of accounts stored in the system.
    /// </summary>
    public class Account
    {
        public int Id { get; set; }

        /// <summary>
        /// 13 chars: number of account
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Any user defined data to recognize account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Account may have following types:
        /// Passive: true,
        /// Active: false,
        /// Active-Passive: null.
        /// </summary>
        //public bool? IsPassive { get; set; }

        /// <summary>
        /// Financial amount 
        /// </summary>
        public Money Money { get; set; }

        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        public int? DepositAccountId { get; set; }

        /// <summary>
        /// Cannot reference both DepositAccount and StandardAccount
        /// </summary>
        public DepositAccount DepositAccount { get; set; }

        public int? StandardAccountId { get; set; }

        /// <summary>
        /// Cannot reference both DepositAccount and StandardAccount
        /// </summary>
        public StandardAccount StandardAccount { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
