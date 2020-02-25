using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    /// <summary>
    /// Describes deposits terms
    /// </summary>
    public class CreditGeneral
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Whether it's possible withdraw all the deposit
        /// before deposit has expired
        /// </summary>
        public bool IsRevocable { get; set; }

        public bool WithCapitalization { get; set; }


        /// <summary>
        /// Can bring additional bankroll(cash) to deposit 
        /// </summary>
        public bool ReplenishmentAllowed { get; set; }
       
        public List<DepositVariable> DepositVariables { get; set; }
    }
}
