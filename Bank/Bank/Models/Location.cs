using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Location
    {
        public int Id { get; set; }

        public City City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }
    }
}
