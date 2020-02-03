using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Birth
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public List<Person> People { get; set; }
    }
}
