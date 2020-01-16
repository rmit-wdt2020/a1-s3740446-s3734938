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
        private AccountRepository AccountRepo = new AccountRepository();
        private TransactionRepository TransactionRepo = new TransactionRepository();
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
            set { transactions = value; }
        }

        public string FormattedBalance
        {
            get { return string.Format("{0:C}", balance); }
        }

        public void Withdraw(decimal amount)
        {
            // Check if remaining balance after withdraw is greater than minimum balance required for the account.
            if (!(Balance -amount >= minimumBalance))
            {
                throw new Exception("Insufficient funds.");
            }
            
            // Decrease existing balance.
            Balance -= amount;
            
            // Make a list of transactions excluding the service type. If this list has four or more transactions
            // charge a withdraw fee.
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceTransaction);
            
            // We add a plus one in the check since we are not counting the initial deposit transaction made when the
            // account opens.
            if (filteredList.Count() >= NumberOfFreeTransactions + 1)
            {
                    // Deduct withdraw service charges and generate a separate transaction for that.
                    Balance -= WithDrawServiceCharge;
                    Console.WriteLine("Service charge of 0.10 incurred");
                    GenerateTransaction(WithDrawServiceCharge,Transaction.ServiceTransaction);
            }
            else
            {
                Console.WriteLine((NumberOfFreeTransactions - filteredList.Count()) + " free transactions left.");
            }

            // Updating the database.
            AccountRepo.Update(this);
            GenerateTransaction(amount, Transaction.WithdrawTransaction);
            
        }

        // Update the balance with deposit amount and generate a transaction.
        public void Deposit(decimal amount)
        {
            balance = balance + amount;
            AccountRepo.Update(this);
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
            TransactionRepo.Insert(transaction);
        }

        // This code block performs a withdraw with transaction type as transfer.
        public void TransferMoney(decimal amount, Account receiverAccount, string comment = "")
        {
            if (!(Balance - amount >= minimumBalance))
            {
                throw new Exception("Insufficient funds.");
            }
            
            Balance -= amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceTransaction);
            
            if (filteredList.Count() >= NumberOfFreeTransactions + 1)
            {
                    Balance -= TransferServiceCharge;
                Console.WriteLine("Service charge of 0.20 incurred");
                GenerateTransaction(TransferServiceCharge,Transaction.ServiceTransaction,receiverAccount.AccountNumber);
                    
            }
            else
            {
                Console.WriteLine((NumberOfFreeTransactions - filteredList.Count()) + " free transactions left.");
            }

            // Update sender account's balance.
            AccountRepo.Update(this);
            GenerateTransaction(amount, Transaction.TransferTransaction,receiverAccount.AccountNumber,comment);

            // Update receiver account's balance.
            receiverAccount.Balance = receiverAccount.Balance + amount;
            AccountRepo.Update(receiverAccount);
            receiverAccount.GenerateTransaction(amount, Transaction.TransferTransaction,0,comment);
            
        }

    }
}