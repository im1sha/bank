using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Person
    {
        public int Id { get; set; }
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        public int BirthId { get; set; }
        public Birth Birth { get; set; }

        public int PassportId { get; set; }
        public Passport Passport { get; set; }

        public List<PersonToLocation> PersonToLocations { get; set; }

        /// <summary>
        /// mask
        /// </summary>
        ///         
        [DisplayName("Домашний телефон")]
        public string HomePhone { get; set; }

        /// <summary>
        /// mask
        /// </summary>
        [DisplayName("Мобильный телефон")]
        public string MobilePhone { get; set; }

        [DisplayName("Электронная почта")]
        public string Email { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }

       // public int? MaritalStatusId { get; set; }
        public bool MaritalStatus { get; set; }

        public int? NationalityId { get; set; }
        public Nationality Nationality { get; set; }

        public int? DisabilityId { get; set; }
        public Disability Disability { get; set; }

        public bool? IsPensioner { get; set; }

        public double Revenue { get; set; }

        //public int ClientId { get; set; }
        //public Client Client { get; set; }
    }
}
