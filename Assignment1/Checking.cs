using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class Checking : Account
    {
    

        public override void Withdraw(decimal amount, char type = 'W')
        {
            decimal atmWithdrawFee = 0.10M;

            if (!(Balance - amount >= 200))
            {
                throw new Exception("Insufficient funds. /n The minimum balance for a checking account is A$200");
            }

            Balance = Balance - amount;

            if (Transactions.Count >= 4)
            {
                Balance = Balance - atmWithdrawFee;
            }

            DatabaseAccess.Instance.UpdateBalance(Balance, AccountNumber);
            GenerateTransaction(amount, type);
        }
    }
}
