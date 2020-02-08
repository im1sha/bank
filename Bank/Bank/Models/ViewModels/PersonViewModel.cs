using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class PersonViewModel
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        #region birth
        public int BirthId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public int? BirthLocationId { get; set; }

        public int BirthCityId { get; set; }

        public string BirthCityName { get; set; }

        #endregion

        //public int PassportId { get; set; }
        //public Passport Passport { get; set; }

        ///// <summary>
        ///// mask
        ///// </summary>
        //public string HomePhone { get; set; }

        ///// <summary>
        ///// mask
        ///// </summary>
        //public string MobilePhone { get; set; }

        //public string Email { get; set; }

        //public int? PostId { get; set; }
        //public Post Post { get; set; }

        //// public int? MaritalStatusId { get; set; }
        //public bool MaritalStatus { get; set; }

        //public int? NationalityId { get; set; }
        //public Nationality Nationality { get; set; }

        //public int? DisabilityId { get; set; }
        //public Disability Disability { get; set; }

        //public bool? IsPensioner { get; set; }

        //public decimal Revenue { get; set; }

    }
}
