using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bank.Models
{
    public class CurrencyAttribute : ValidationAttribute
    {
        private readonly bool _required;

        public CurrencyAttribute(bool required)
        {
            _required = required;
        }

        public override bool IsValid(object value)
        {
            var inputValue = value as string;

            if (string.IsNullOrEmpty(inputValue) && !_required)
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(inputValue))
            {
                static bool IsDigit(char code)
                {
                    return code >= '0' && code <= '9';
                }

                var separators = new[] { '.', ',' };

                // Format :
                // 123456.00 | 12345 | 0.45 | 0.0 
                var result = inputValue == "0" 
                    || ((inputValue.Count(i => separators.Contains(i)) <= 1)
                        && (inputValue.Count(i => separators.Contains(i)) + inputValue.Count(i => IsDigit(i)) == inputValue.Length)
                        && !separators.Any(i => inputValue.StartsWith(i))
                        && !separators.Any(i => inputValue.EndsWith(i))
                        && (inputValue.Length <= 4 || !inputValue.Any(i => separators.Contains(i)) || separators.Any(i => inputValue.TakeLast(3).Contains(i)))
                        && (inputValue.First() != '0' || (inputValue.Length > 1 && separators.Contains(inputValue[1])))
                        && decimal.TryParse(inputValue, out _));
                return result;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Field {name} should have one of formats: 123456789.00, 123456789";
        }
    }
}

