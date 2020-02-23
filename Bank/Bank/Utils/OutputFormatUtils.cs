using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public static class OutputFormatUtils
    {
        public static string ConvertBoolToYesNoFormat(bool x)
        {
            return x ? "Yes" : "No";
        }       
    }
}
