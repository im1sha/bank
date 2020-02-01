using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Passport
    {
        public int Id { get; set; }

        /// <summary>
        /// Add mask here later
        /// </summary>
        public string Number { get; set; }

        public string Series { get; set; }

        /// <summary>
        /// Add mask here later
        /// </summary>
        public string IdentifyingNumber { get; set; }


        public int IssuingAuthorityId { get; set; }
        public IssuingAuthority IssuingAuthority { get; set; }


        public DateTime IssuingDate { get; set; }
    }
}
