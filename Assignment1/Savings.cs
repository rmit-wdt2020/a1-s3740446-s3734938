using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class Savings : Account
    {

        public override void Withdraw(decimal amount, char type = 'W')
        {
            decimal atmWithdrawFee = 0.10M;

            if (!(Balance >= amount))
            {
                throw new Exception("Insufficient funds.");
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
