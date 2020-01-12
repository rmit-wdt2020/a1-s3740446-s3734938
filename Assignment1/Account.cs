using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public class Account
    {
        private int accountNumber;
        private char accountType;
        private string customerId;      
        private decimal balance;
        private List<Transaction> transactions = new List<Transaction>();  

        public int AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public char AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        public string CustomerId
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

        public Account()
        {
        }
        
        public void withdraw(decimal amount)
        {
            decimal atmWithdrawFee = 0.10M;

            if(!(balance >= amount)) 
            {
               throw new Exception("Insufficient funds.");
            }

            if(!(balance - amount >= 200) && accountType == 'C') 
            {
               throw new Exception("The minimum balance for a checking account is A$200 ");
            }
            
             balance = balance - amount;

             if(transactions.Count >= 4)
             {
                 balance = balance - atmWithdrawFee;
             }
             
             DatabaseAccess.Instance.updateBalance(balance,this.accountNumber);
             generateTransaction(amount,'W');
        }
        public void deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.updateBalance(balance,this.accountNumber);
            generateTransaction(amount,'D');
        }
        
        public void generateTransaction(decimal amount,char transactionType)
        {
            Transaction transaction = new Transaction(){
            TransactionType = transactionType,
            AccountNumber = this.accountNumber,
            Amount = amount,
            TransactionTimeUtc = DateTime.UtcNow
            };

            transactions.Add(transaction);
            DatabaseAccess.Instance.insertTransaction(transaction);
        }

    }
}