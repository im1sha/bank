using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class PersonFullViewModel
    {
        public int Id { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Middle name")]
        public string MiddleName { get; set; }

        #region birth

        public int BirthId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Birth date")]
        public DateTime? BirthDate { get; set; }

        public int? BirthLocationId { get; set; }

        public int? BirthLocationCityId { get; set; }

        [DisplayName("Birth city")]
        public string BirthLocationCityName { get; set; }

        #endregion

        #region passport

        public int PassportId { get; set; }

        [DisplayName("Passport number")]
        public string PassportNumber { get; set; }

        [DisplayName("Passport series")]
        public string PassportSeries { get; set; }

        [DisplayName("Identifying number")]
        public string PassportIdentifyingNumber { get; set; }

        public int PassportIssuingAuthorityId { get; set; }

        [DisplayName("Issuing authority")]
        public string PassportIssuingAuthorityName { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Issuing date")]
        public DateTime PassportIssuingDate { get; set; }

        #endregion

        [DisplayName("Home phone")]
        public string HomePhone { get; set; }

        [DisplayName("Mobile phone")]
        public string MobilePhone { get; set; }

        public string Email { get; set; }

        #region company

        public int? PostId { get; set; }

        [DisplayName("Post")]
        public string PostName { get; set; }

        public int? CompanyId { get; set; }

        [DisplayName("Company")]
        public string CompanyName { get; set; }

        #endregion

        [DisplayName("Married")]
        public bool MaritalStatus { get; set; }

        #region nationality

        public int? NationalityId { get; set; }

        [DisplayName("Nationality")]
        public string NationalityName { get; set; }
        #endregion

        #region disability

        public int? DisabilityId { get; set; }

        [DisplayName("Disability")]
        public string DisabilityName { get; set; }

        #endregion

        [DisplayName("Pensioner")]
        public bool? IsPensioner { get; set; }

        public decimal Revenue { get; set; }

    }
}


