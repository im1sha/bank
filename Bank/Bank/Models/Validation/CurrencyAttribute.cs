using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class CurrencyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrEmpty(inputValue))
            {
                static bool IsDigit(char code) => code >= '0' && code <= '9' ;

                var separators = new[] { '.', ',' };

                // Format :
                // 123456.00 | 12345 | 0.45 | 0.0 
                return (inputValue.Length > 0 && inputValue.Count(i => separators.Contains(i)) <= 1)
                    && !separators.Any(i => inputValue.StartsWith(i))
                    && !separators.Any(i => inputValue.EndsWith(i))                    
                    && (inputValue.Count(i => separators.Contains(i)) == 0 || inputValue.First() != '0' || (inputValue.Length > 1 && !IsDigit(inputValue[1])))
                    && (inputValue.Count(i => separators.Contains(i)) + inputValue.Count(i => IsDigit(i)) == inputValue.Length)
                    && (inputValue.Length <= 4 || !separators.Contains(inputValue.TakeLast(4).First()));
            }

            return isValid;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Field {name} should have one of formats: 123456789.00, 123456789, 0.99, 0.0 ";
        }
    }
}

