using System;
namespace BankingApplication
{
    public class Transaction
    {
        private int transactionId;
        private char transactionType;
        private int accountNumber;
        private int destinationAccountNumber;
        private decimal amount;
        private string comment;
        private DateTime transactionTimeUtc; //check string or datetime

        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }

        public char TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        public int AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public int DestinationAccountNumber
        {
            get { return destinationAccountNumber; }
            set { destinationAccountNumber = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public DateTime TransactionTimeUtc
        {
            get { return transactionTimeUtc; }
            set { transactionTimeUtc = value; }
        }

        public Transaction(string transactionTimeUtc )
        {
            this.TransactionTimeUtc = DateTime.Parse(transactionTimeUtc);
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