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

        #region locations

        public int ResidenceLocationId { get; set; }
        public int ResidenceLocationCityId { get; set; }       
        [DisplayName("Registration city")]
        public string ResidenceLocationCity { get; set; }
        [DisplayName("Registration street")]
        public string ResidenceLocationStreet { get; set; }
        [DisplayName("Registration building")]
        public string ResidenceLocationBuildingNumber { get; set; }

        public int ActualLocationId { get; set; }
        public int ActualLocationCityId { get; set; }
        [DisplayName("Actual city")]
        public string ActualLocationCity { get; set; }
        [DisplayName("Actual street")]
        public string ActualLocationStreet { get; set; }
        [DisplayName("Actual building")]
        public string ActualLocationBuildingNumber { get; set; }

        #endregion

        [DisplayName("Home phone")]
        public string HomePhone { get; set; }

        [DisplayName("Mobile phone")]
        public string MobilePhone { get; set; }

        public string Email { get; set; }

        #region company

        public int? PostId { get; set; }

        [DisplayName("Position")]
        public string PostName { get; set; }

        public int? CompanyId { get; set; }

        [DisplayName("Job")]
        public string CompanyName { get; set; }

        #endregion

        [DisplayName("Married")]
        public bool? MaritalStatus { get; set; }

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
        public bool IsPensioner { get; set; }

        public decimal Revenue { get; set; }

    }
}


