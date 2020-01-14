using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BankingApplication
{
    class Savings : Account
    {
        public Savings()
        {
            minimumBalance = 0;
        }
        public override void Withdraw(decimal amount)
        {

            if (!(Balance -amount >= minimumBalance))
            {
                throw new Exception("Insufficient funds.");
            }

            Balance = Balance - amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceTransaction);
            
            if (filteredList.Count() >= NumberOfFreeTransactions + 1)
            {
                Balance = Balance - WithDrawServiceCharge;
                GenerateTransaction(WithDrawServiceCharge,Transaction.ServiceTransaction);
            }

            DatabaseAccess.Instance.UpdateBalance(Balance, AccountNumber);
            GenerateTransaction(amount, Transaction.WithdrawTransaction);
            
        }
    }
}
