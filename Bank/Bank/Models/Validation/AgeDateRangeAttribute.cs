using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class AgeDateRangeAttribute : RangeAttribute
    {
        public AgeDateRangeAttribute()
            : base(typeof(DateTime),
                  DateTime.Now.AddYears(-150).ToString("d"),
                  DateTime.Now.ToString("d"))
        {
        }

        //public override string FormatErrorMessage(string name)
        //{
        //    return $"The field {name} must be between {Minimum as} and {}.";
        //}

    }
}
