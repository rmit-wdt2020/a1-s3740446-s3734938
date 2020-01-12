using System;

namespace BankingApplication
{     
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string validData) : base("Invalid data entered." + validData + ".")
       {

       }
    }
}