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
        private DateTime transactionTimeUtc; 

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

        public Transaction()
        {

        }
    }
}



    