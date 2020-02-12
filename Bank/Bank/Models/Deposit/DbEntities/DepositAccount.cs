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
    public class DepositAccount
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int DepositVariableId { get; set; }

        public DepositVariable DepositVariable { get; set; }
    }
}
