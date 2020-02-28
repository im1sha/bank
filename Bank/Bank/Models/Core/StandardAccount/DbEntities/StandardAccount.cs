using System.Collections.Generic;

namespace Bank.Models
{
    /// <summary>
    /// Stores financials
    /// </summary>
    public class StandardAccount
    {
        public int Id { get; set; }

        
        public int? PersonId { get; set; }

        /// <summary>
        /// Cannot have both Person and LegalEntity owner references
        /// </summary>
        public Person Person { get; set; }

        public int? LegalEntityId { get; set; }

        /// <summary>
        /// Cannot have both Person and LegalEntity owner references
        /// </summary>
        public LegalEntity LegalEntity { get; set; }

        public Account Account { get; set; }

        /// <summary>
        /// Destinations
        /// </summary>
        public List<CreditAccount> CreditAccountDestinations { get; set; }
    }
}
