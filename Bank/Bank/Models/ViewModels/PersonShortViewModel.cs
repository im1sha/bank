using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class PersonShortViewModel
    {
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Middle name")]
        public string MiddleName { get; set; }

        public int PassportId { get; set; }

        [DisplayName("Passport number")]
        public string PassportNumber { get; set; }

        public string PassportSeries { get; set; }
    }
}
