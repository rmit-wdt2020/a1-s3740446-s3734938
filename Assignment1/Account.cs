using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingApplication
{
    public abstract class Account
    {
        protected const int NumberOfFreeTransactions = 4;
        protected const decimal WithDrawServiceCharge = 0.1M;
        protected const decimal TransferServiceCharge = 0.2M;
        protected decimal minimumBalance;
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

        public void Withdraw(decimal amount)
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
        public void Deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.UpdateBalance(balance,this.accountNumber);
            GenerateTransaction(amount, Transaction.DepositTransaction);
        }
        
        public void GenerateTransaction(decimal amount,char transactionType)
        {
            Transaction transaction = new Transaction(){
            TransactionType = transactionType,
            AccountNumber = this.accountNumber,
            Amount = amount,
            TransactionTimeUtc = DateTime.UtcNow
            };

            transactions.Add(transaction);
            DatabaseAccess.Instance.InsertTransaction(transaction);
        }

    }
}