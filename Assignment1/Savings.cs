using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class Savings : IAccount
    {
        private int accountNumber;
        private int customerId;
        private decimal balance;

        private List<Transaction> transactions = new List<Transaction>();


        public int AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }


        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public List<Transaction> Transactions
        {
            get { return transactions; }
            set { }
        }

        public void Withdraw(decimal amount, char type = 'W')
        {
            decimal atmWithdrawFee = 0.10M;

            if (!(balance >= amount))
            {
                throw new Exception("Insufficient funds.");
            }

            balance = balance - amount;

            if (transactions.Count >= 4)
            {
                balance = balance - atmWithdrawFee;
            }

            DatabaseAccess.Instance.UpdateBalance(balance, accountNumber);
            GenerateTransaction(amount, type);
        }
        public void Deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.UpdateBalance(balance, this.accountNumber);
            GenerateTransaction(amount, 'D');
        }

        public void GenerateTransaction(decimal amount, char transactionType)
        {
            Transaction transaction = new Transaction()
            {
                TransactionType = transactionType,
                AccountNumber = accountNumber,
                Amount = amount,
                TransactionTimeUtc = DateTime.UtcNow
            };

            transactions.Add(transaction);
            DatabaseAccess.Instance.InsertTransaction(transaction);
        }
    }
}
