using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class AtmTransactionViewModel
    {
        public string Target { get; set; } 

        public string Date { get; set; }

        public string Amount { get; set; }
    }
}
