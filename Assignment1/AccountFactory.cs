using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    //Factory pattern for creating Account objects

    static class AccountFactory
    {
        public static Account CreateAccount(int AccountNumber, char Type, int CustomerId, decimal Balance)
        {
            if(Type == 'S')
            {
                return new Savings { AccountNumber = AccountNumber, CustomerId = CustomerId, Balance = Balance };
            }
            else
            {
                return new Checking { AccountNumber = AccountNumber, CustomerId = CustomerId, Balance = Balance };
            }
        }
    }
}
