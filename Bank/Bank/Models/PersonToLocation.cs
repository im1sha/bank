using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class PersonToLocation
    {
        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }
        
        public bool IsActual { get; set; }
    }
}
