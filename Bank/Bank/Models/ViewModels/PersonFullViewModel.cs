using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class PersonFullViewModel
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

        public int? BirthLocationCityId { get; set; }

        public string BirthLocationCityName { get; set; }

        #endregion

        #region passport

        public int PassportId { get; set; }

        public string PassportNumber { get; set; }

        public string PassportSeries { get; set; }

        public string PassportIdentifyingNumber { get; set; }

        public int PassportIssuingAuthorityId { get; set; }

        public string PassportIssuingAuthorityName { get; set; }

        [DataType(DataType.Date)]
        public DateTime PassportIssuingDate { get; set; }

        #endregion

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        #region company

        public int? PostId { get; set; }

        public string PostName { get; set; }

        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }

        #endregion

        public bool MaritalStatus { get; set; }

        #region nationality

        public int? NationalityId { get; set; }

        public string NationalityName { get; set; }
        #endregion

        #region disability

        public int? DisabilityId { get; set; }

        public string DisabilityName { get; set; }

        #endregion

        public bool? IsPensioner { get; set; }

        public decimal Revenue { get; set; }

    }
}


