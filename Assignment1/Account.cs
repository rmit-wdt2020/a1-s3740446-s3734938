using System;
using System.Collections.Generic;

namespace BankingApplication
{
    //public enum Type
    //{
    //    Savings,
    //    Checking
    //}

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

        public void InitTransaction()
        {
            Transactions[0].TransactionId = 1;
            Transactions[0].TransactionType = 'D';
            Transactions[0].Amount = Balance;
            Transactions[0].AccountNumber = AccountNumber;
            Transactions[0].DestinationAccountNumber = AccountNumber;
        }

        public void withdraw(decimal amount)
        {
            if(balance >= amount ) 
            {
                balance = balance - amount;
                DatabaseAccess.Instance.updateBalance(balance,this.accountNumber);
                Console.WriteLine(balance);
            }
            else 
            {
                Console.WriteLine("Insufficient funds");
            }
        }

        public void deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.updateBalance(balance,this.accountNumber);
            Console.WriteLine(balance);
        }
    }
}


    //AccountNumber int not null,
    //AccountType char not null,
    //CustomerID int not null,
    //Balance money not null,
    //constraint PK_Account primary key (AccountNumber),
    //constraint FK_Account_Customer foreign key (CustomerID) references Customer (CustomerID),
    //constraint CH_Account_AccountType check (AccountType in ('C', 'S')),
    //constraint CH_Account_Balance check(Balance >= 0)