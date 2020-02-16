using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class StandardAccountIndexViewModel
    {

        public int Id { get; set; }

        [DisplayName("Account number")]
        public string Number { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Active")]
        public string IsActive { get; set; }

        public int? PersonId { get; set; }
        public int? LegalEntityId { get; set; }

        public bool IsPerson{ get; set; }
        public string Owner { get; set; }

        public string Passport { get; set; }

        [DisplayName("Opened")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }

        [DisplayName("Terminated")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }
    }
}
