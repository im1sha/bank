using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// Plan of account(план учета) : list of accounts stored in the system
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

        // entities of this calls required to serve deposits

        // data required to calculate deposits correctly
        // mb just rerefences to deposits


    }
}
