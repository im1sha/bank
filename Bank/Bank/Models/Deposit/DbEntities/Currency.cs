using System.Collections.Generic;

namespace Bank.Models
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DepositVariable> DepositVariables { get; set; }
    }
}
