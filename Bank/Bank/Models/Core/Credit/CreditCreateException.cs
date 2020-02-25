//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Bank
//{
//    public enum CreditCreateExceptionType
//    { 
//        PersonNotExist,
//        StandardAccountsNotExist,
//        AccountsOfGivenCurrencyNotExist,
//        DepositNotExist,
//        InterestAccrualNotFound,
//        NotEnoughOfMoney,
//    }

//    public class CreditCreateException : ApplicationException
//    {
//        public CreditCreateException(CreditCreateExceptionType reason)
//        {
//            Reason = reason;
//        }
//        public CreditCreateExceptionType Reason { get; }
//    }
//}
