using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Birth Birth { get; set; }

        public Passport Passport { get; set; }

        public Location ActualLocation { get; set; }
     
        public Location ResidenceLocation { get; set; }

        /// <summary>
        /// mask
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// mask
        /// </summary>
        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public Post Post { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

        public Nationality Nationality { get; set; }

        public Disability Disability { get; set; }

        public bool IsPensioner { get; set; }

        public decimal Revenue { get; set; }
    }
}
