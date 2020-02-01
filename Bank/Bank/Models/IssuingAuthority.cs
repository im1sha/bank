using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class IssuingAuthority
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }
    }
}
