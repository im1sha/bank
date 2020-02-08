using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Location
    {
        public int Id { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public List<Company> Companies { get; set; }

        public List<Birth> Births { get; set; }

        public List<PersonToLocation> PersonToLocations { get; set; }

        public List<IssuingAuthority> IssuingAuthorities { get; set; }
    }
}
