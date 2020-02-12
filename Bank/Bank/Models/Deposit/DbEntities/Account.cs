using System.Collections.Generic;

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

        public bool IsPassive { get; set; }

        public decimal TotalFund { get; set; }

        /// <summary>
        /// Cannot reference both DepositAccount and StandardAccount
        /// </summary>
        public DepositAccount DepositAccount { get; set; }

        /// <summary>
        /// Cannot reference both DepositAccount and StandardAccount
        /// </summary>
        public StandardAccount StandardAccount { get; set; }

        public List<Log> Logs { get; set; }
    }
}
