using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public enum DepositCreateExceptionType
    { 
        PersonNotExist,
        StandardAccountsNotExist,
        AccountsOfGivenCurrencyNotExist,
        DepositNotExist,
        InterestAccrualNotFound,
    }

    public class DepositCreateException : ApplicationException
    {
        public DepositCreateException(DepositCreateExceptionType reason)
        {
            Reason = reason;
        }
        public DepositCreateExceptionType Reason { get; }
    }
}
