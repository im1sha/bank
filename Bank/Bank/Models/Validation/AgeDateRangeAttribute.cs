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
    }
}
