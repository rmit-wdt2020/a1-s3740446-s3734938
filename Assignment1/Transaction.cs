using System;
namespace HelloWorldApplication
{
    public class Transaction
    {
        private int transactionId;
        private char transactionType;
        private int accountNumber;
        private int destinationAccountNumber;
        private double money;
        private string comment;
        private DateTime transactionTimeUtc; //check string or datetime

        public int TransactionId
        {
            get { return transactionId; }
        }

        public char TransactionType
        {
            get { return transactionType; }
        }

        public int AccountNumber
        {
            get { return accountNumber; }
        }

        public int DestinationAccountNumber
        {
            get { return destinationAccountNumber; }
        }

        public double Money
        {
            get { return money; }
        }

        public string Comment
        {
            get { return comment; }
        }

        public DateTime TransactionTimeUtc
        {
            get { return transactionTimeUtc; }
        }

        public Transaction()
        {
        }
    }
}



    //TransactionID int identity not null,
    //TransactionType char not null,
    //AccountNumber int not null,
    //DestinationAccountNumber int null,
    //Amount money not null,
    //Comment nvarchar(255) null,
    //TransactionTimeUtc datetime2 not null,