using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class AgeDateRangeAttribute : RangeAttribute
    {
        public AgeDateRangeAttribute(int deltaYears = 150)
            : base(typeof(DateTime),
                  DateTime.Now.AddYears(-deltaYears).ToString("d"),
                  DateTime.Now.ToString("d"))
        {
        }
    }
}
