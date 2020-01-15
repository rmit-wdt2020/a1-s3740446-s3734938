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
            // Check if remaining balance after withdraw is greater than minimum balance required for the account.
            if (!(Balance -amount >= minimumBalance))
            {
                throw new Exception("Insufficient funds.");
            }
            
            // Decrease existing balance.
            Balance = Balance - amount;
            
            // Make a list of transactions excluding the service type. If this list has four or more transactions
            // charge a withdraw fee.
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceTransaction);
            
            // We add a plus one in the check since we are not counting the initial deposit transaction made when the
            // account opens.
            if (filteredList.Count() >= NumberOfFreeTransactions + 1)
            {
                    // Deduct withdraw service charges and generate a separate transaction for that.
                    Balance = Balance - WithDrawServiceCharge;
                    GenerateTransaction(WithDrawServiceCharge,Transaction.ServiceTransaction);
            }

            // Updating the database.
            DatabaseAccess.Instance.UpdateBalance(Balance, AccountNumber);
            GenerateTransaction(amount, Transaction.WithdrawTransaction);
            
        }

        // Update the balance with deposit amount and generate a transaction.
        public void Deposit(decimal amount)
        {
            balance = balance + amount;
            DatabaseAccess.Instance.UpdateBalance(balance,this.accountNumber);
            GenerateTransaction(amount, Transaction.DepositTransaction);
        }
        
        // Making a transaction object, adding it to list of transactions of the current account and also inserting it in database.
        // Destination account number and comment parameters overrided during a transfer. Comment override is optional since customer may not
        // enter a comment.
        public void GenerateTransaction(decimal amount,char transactionType,int destinationAccountNumber = 0,string comment = "")
        {
            Transaction transaction = new Transaction(){
            TransactionType = transactionType,
            AccountNumber = this.accountNumber,
            Amount = amount,
            TransactionTimeUtc = DateTime.UtcNow,
            DestinationAccountNumber = destinationAccountNumber,
            Comment = comment
            };

            transactions.Add(transaction);
            DatabaseAccess.Instance.InsertTransaction(transaction);
        }

        public void TransferMoney(decimal amount, Account receiverAccount,string comment = "")
        {
            // This code block performs a withdraw with transaction type as transfer.
            if (!(Balance - amount >= minimumBalance))
            {
                throw new Exception("Insufficient funds.");
            }
            
            Balance = Balance - amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceTransaction);
            
            if (filteredList.Count() >= NumberOfFreeTransactions + 1)
            {
                    Balance = Balance - TransferServiceCharge;
                    GenerateTransaction(TransferServiceCharge,Transaction.ServiceTransaction,receiverAccount.AccountNumber);
                
            }

            // Update sender account's balance.
            DatabaseAccess.Instance.UpdateBalance(Balance, AccountNumber);
            GenerateTransaction(amount, Transaction.TransferTransaction,receiverAccount.AccountNumber,comment);

            // Update receiver account's balance.
            receiverAccount.Balance = receiverAccount.Balance + amount;
            DatabaseAccess.Instance.UpdateBalance(receiverAccount.Balance,receiverAccount.AccountNumber);
            receiverAccount.GenerateTransaction(amount, Transaction.TransferTransaction,0,comment);
            
        }

    }
}