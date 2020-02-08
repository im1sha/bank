using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class PassportIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrEmpty(inputValue))
            {
                static bool IsLetter(char code) => code >= 'a' && code <= 'z' || code >= 'A' && code <= 'Z';

                // Format :
                // 1234567 A 123 AB 1
                return (inputValue.Length == 14
                    && int.TryParse(inputValue.Take(7).ToString(), out _)
                    && int.TryParse(inputValue.Skip(8).Take(3).ToString(), out _)
                    && int.TryParse(inputValue.Skip(13).Take(1).ToString(), out _)
                    && IsLetter(inputValue[7])
                    && IsLetter(inputValue[11])
                    && IsLetter(inputValue[12]));
            }

            return isValid;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"Field {name} should have following format 1234567A123AB1";
        }
    }
}

