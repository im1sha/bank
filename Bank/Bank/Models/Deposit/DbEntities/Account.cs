using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Plan of account(план учета) : list of accounts stored in the system.
    /// Entities of this class are required to serve deposits.
    /// Data required to calculate deposits correctly.
    /// It just may rerefence to deposit.
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

        public DepositAccount DepositAccount { get; set; }

        public List<Log> Logs { get; set; }
    }
}
