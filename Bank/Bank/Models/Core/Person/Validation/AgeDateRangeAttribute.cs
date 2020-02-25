//using System;
//using System.ComponentModel.DataAnnotations;

//namespace Bank.Models
//{
//    public class AgeDateRangeAttribute : RangeAttribute
//    {
//        public AgeDateRangeAttribute(int fromYear, int fromMonth, int toYear, int toMonth)
//            : base(typeof(DateTime),
//                  DateTime.Now.AddYears(-fromYear).AddMonths(-fromMonth).ToString("d"),
//                  DateTime.Now.AddYears(toYear).AddMonths(toMonth).ToString("d"))
//        {
//        }
//    }
//}
