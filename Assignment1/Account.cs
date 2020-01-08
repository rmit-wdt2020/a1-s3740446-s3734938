using System;
namespace HelloWorldApplication
{
    public enum Type
    {
        Savings,
        Checking
    }

    public class Account
    {
        private int accountNumber;
        private Type accountType;
        private Customer customer;      
        private double balance;

        public int AccountNumber
        {
            get { return accountNumber; }
        }

        public Type AccountType
        {
            get { return accountType; }
        }

        public Customer Customer
        {
            get { return customer; }
        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }

        }

        public Account()
        {
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