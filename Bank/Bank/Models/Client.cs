using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Client
    {
        public int Id { get; set; }
    
        public Person Person { get; set; }
    }
}
