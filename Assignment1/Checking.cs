using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class Checking : IAccount
    {
        private int accountNumber;
        //private char accountType;
        private int customerId;
        private decimal balance;

        private List<Transaction> transactions = new List<Transaction>();

        public int AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        //public char AccountType
        //{
        //    get { return accountType; }
        //    set { accountType = value; }
        //}

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

        public void withdraw(decimal amount, char type = 'W')
        {
            decimal atmWithdrawFee = 0.10M;

            if (!(balance - amount >= 200))
            {
                throw new Exception("Insufficient funds. /n The minimum balance for a checking account is A$200");
            }

            balance = balance - amount;

            if (transactions.Count >= 4)
            {
                balance = balance - atmWithdrawFee;
            }

            DatabaseAccess.Instance.updateBalance(balance, accountNumber);
            generateTransaction(amount, type);
        }
        public void deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.updateBalance(balance, this.accountNumber);
            generateTransaction(amount, 'D');
        }

        public void generateTransaction(decimal amount, char transactionType)
        {
            Transaction transaction = new Transaction()
            {
                TransactionType = transactionType,
                AccountNumber = accountNumber,
                Amount = amount,
                TransactionTimeUtc = DateTime.UtcNow
            };

            transactions.Add(transaction);
            DatabaseAccess.Instance.insertTransaction(transaction);
        }
    }
}
