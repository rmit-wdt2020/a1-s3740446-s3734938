using System;

namespace BankingApplication
{  
    //Custom exception for invalid data entry in the console driver class
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string validData) : base("Invalid data entered." + validData + ".")
       {

       }
    }
}