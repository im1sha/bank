using System.Collections.Generic;

namespace Bank.Models
{
    /// <summary>
    /// Transaction items
    /// </summary>
    public class LegalEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<StandardAccount> StandardAccounts { get; set; }
    }
}
