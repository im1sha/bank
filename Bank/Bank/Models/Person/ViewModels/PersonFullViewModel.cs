using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class PersonFullViewModel
    {
        public int? Id { get; set; }

        [Required]
        [DisplayName("First name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only.")]
        public string FirstName { get; set; } = "Name";

        [Required]
        [DisplayName("Last name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only.")]
        public string LastName { get; set; } = "Surname";

        [Required]
        [DisplayName("Middle name")]
        [RegularExpression(@"^([\-'a-zA-Z])+$", ErrorMessage = "You should use letters and characters \"-\", \"'\"  only.")]
        public string MiddleName { get; set; } = "Middlename";

        #region birth

        public int? BirthId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Birth date")]
        [AgeDateRange]
        public DateTime? BirthDate { get; set; } = new DateTime(1999, 4, 4);

        public int? BirthLocationId { get; set; }

        [Required]
        [DisplayName("Birth city")]
        public int? BirthLocationCityId { get; set; } = 1;

        [DisplayName("Birth city")]
        public List<City> BirthLocationCityList { get; set; }

        [DisplayName("Birth city")]
        public string BirthLocationCityName { get; set; }

        #endregion

        #region passport

        public int? PassportId { get; set; }

        [Required]
        [DisplayName("Passport number")]
        [StringLength(7)]
        [RegularExpression(@"^([0-9])+$", ErrorMessage = "Number should contain digits only.")]
        public string PassportNumber { get; set; } = "1111111";

        //[Remote(action: "CheckPassportSeriesAndNumber", controller: "Person", ErrorMessage = "Passport series and number should be unique")]
        //public (string Series, string Number, int? PersonId) PassportSeriesAndNumber { get; set; }

        [Required]
        [StringLength(2)]
        [RegularExpression(@"^([A-Z])+$", ErrorMessage = "Series should contain capital latin letters only.")]
        [DisplayName("Passport series")]
        public string PassportSeries { get; set; } = "AA";

        //[Remote(action: "CheckPassportIdentifyingNumber", controller: "Person", ErrorMessage = "Passport identifying number should be unique")]
        //public (string IdentifyingNumber, int? PersonId) PassportIdentifyingNumberAndPersonId { get; set; }

        [Required]
        [StringLength(14)]
        [PassportId]
        [DisplayName("Identifying number")]
        public string PassportIdentifyingNumber { get; set; } = "1111111A123AA2";

        public int? PassportIssuingAuthorityId { get; set; }

        [Required]
        [DisplayName("Issuing authority")]
        public string PassportIssuingAuthorityName { get; set; } = "PIA-2";

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Issuing date")]
        [AgeDateRange]
        public DateTime? PassportIssuingDate { get; set; } = new DateTime(2018, 5, 5);

        #endregion

        #region locations

        public int? RegistrationLocationId { get; set; }

        [Required]
        [DisplayName("Registration city")]
        public int? RegistrationLocationCityId { get; set; } = 1;

        [DisplayName("Registration city")]
        public List<City> RegistrationLocationCityList { get; set; }

        [DisplayName("Registration city")]
        public string RegistrationLocationCityName { get; set; }

        [Required]
        [DisplayName("Street")]
        public string RegistrationLocationStreet { get; set; } = "Street 1";

        [Required]
        [DisplayName("Building")]
        public string RegistationLocationBuildingNumber { get; set; } = "25";

        public int? ActualLocationId { get; set; }

        [Required]
        [DisplayName("Actual city")]
        public int? ActualLocationCityId { get; set; } = 1;

        [DisplayName("Actual city")]
        public List<City> ActualLocationCityList { get; set; }
        [DisplayName("Actual city")]
        public string ActualLocationCityName { get; set; }

        [Required]
        [DisplayName("Street")]
        public string ActualLocationStreet { get; set; } = "Street 2";

        [Required]
        [DisplayName("Building")]
        public string ActualLocationBuildingNumber { get; set; } = "345";

        #endregion

        [DisplayName("Home phone")]
        [StringLength(12)]
        [RegularExpression(@"^([0-9])+$", ErrorMessage = "Phone should contain digits only.")]
        public string HomePhone { get; set; } = "123456789012";

        [DisplayName("Mobile phone")]
        [StringLength(12)]
        [RegularExpression(@"^([0-9])+$", ErrorMessage = "Phone should contain digits only.")]
        public string MobilePhone { get; set; } = "123456789013";

        [EmailAddress]
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
        public int? MaritalStatusId { get; set; } = 1;

        [DisplayName("Married")]
        public List<MaritalStatusLocal> MaritalStatusList { get; set; }

        [DisplayName("Married")]
        public string MaritalStatusName { get; set; }

        #region nationality

        [Required]
        [DisplayName("Nationality")]
        public int? NationalityId { get; set; } = 1;

        [DisplayName("Nationality")]
        public List<Nationality> NationalityList { get; set; }

        [DisplayName("Nationality")]
        public string NationalityName { get; set; }

        #endregion

        #region disability

        [Required]
        [DisplayName("Disability")]
        public int? DisabilityId { get; set; } = 1;

        [DisplayName("Disability")]
        public List<Disability> DisabilityList { get; set; }

        [DisplayName("Disability")]
        public string DisabilityName { get; set; }

        #endregion

        [Required]
        [DisplayName("Pensioner")]
        public bool IsPensioner { get; set; } = false;

        [Currency(false)]
        public string Revenue { get; set; }

    }
}


