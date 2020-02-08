using Microsoft.AspNetCore.Mvc.Rendering;
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
        public int? Id { get; set; }

        [Required]
        [DisplayName("First name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Middle name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only")]
        public string MiddleName { get; set; }

        #region birth

        public int? BirthId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Birth date")]
        [AgeDateRange]
        public DateTime? BirthDate { get; set; }

        public int? BirthLocationId { get; set; }

        [Required]      
        [DisplayName("Birth city")]
        public int? BirthLocationCityId { get; set; }

        [DisplayName("Birth city name")]
        public List<City> BirthLocationCityName { get; set; }

        #endregion

        #region passport

        public int? PassportId { get; set; }

        [Required]
        [DisplayName("Passport number")]
        public string PassportNumber { get; set; }

        [Required]
        [DisplayName("Passport series")]
        public string PassportSeries { get; set; }

        [Required]
        [DisplayName("Identifying number")]
        public string PassportIdentifyingNumber { get; set; }

        public int? PassportIssuingAuthorityId { get; set; }

        [Required]
        [DisplayName("Issuing authority")]
        public string PassportIssuingAuthorityName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Issuing date")]
        [AgeDateRange]
        public DateTime? PassportIssuingDate { get; set; }

        #endregion

        #region locations

        public int? RegistrationLocationId { get; set; }

        [Required]
        [DisplayName("Registration city")]
        public int? RegistrationLocationCityId { get; set; }

        [DisplayName("Registration city name")]
        public List<City> RegistrationLocationCity { get; set; }

        [Required]
        [DisplayName("Street")]
        public string RegistrationLocationStreet { get; set; }

        [Required]
        [DisplayName("Building")]
        public string RegistationLocationBuildingNumber { get; set; }

        public int? ActualLocationId { get; set; }

        [Required]
        [DisplayName("Registration city")]
        public int? ActualLocationCityId { get; set; }

        [DisplayName("Registration city name")]
        public List<City> ActualLocationCity { get; set; }

        [Required]
        [DisplayName("Street")]
        public string ActualLocationStreet { get; set; }

        [Required]
        [DisplayName("Building")]
        public string ActualLocationBuildingNumber { get; set; }

        #endregion

        [DisplayName("Home phone")]
        public string HomePhone { get; set; }

        [DisplayName("Mobile phone")]
        public string MobilePhone { get; set; }

        public string Email { get; set; }

        #region company

        public int? PostId { get; set; }

        [DisplayName("Job: position")]
        public string PostName { get; set; }

        public int? CompanyId { get; set; }

        [DisplayName("Job: company")]
        public string CompanyName { get; set; }

        #endregion

        [Required]
        [DisplayName("Married")]
        public int? MaritalStatusId { get; }

        [DisplayName("Married status")]
        public List<MaritalStatusLocal> MaritalStatus { get; set; }

        #region nationality

        [Required]
        [DisplayName("Nationality")]
        public int? NationalityId { get; set; }

        [DisplayName("Nationality name")]
        public List<Nationality> NationalityName { get; set; }
        #endregion

        #region disability

        [Required]
        [DisplayName("Disability")]
        public int? DisabilityId { get; set; }

        [DisplayName("Disability name")]
        public List<Disability> DisabilityName { get; set; }

        #endregion

        [Required]
        [DisplayName("Pensioner")]
        public bool IsPensioner { get; set; }

        public decimal? Revenue { get; set; }

    }
}


