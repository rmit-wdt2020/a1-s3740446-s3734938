using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public interface IAccount
    {
        int AccountNumber { get; set; }
        //char AccountType { get; set; }
        int CustomerId { get; set; }
        decimal Balance { get; set; }
        List<Transaction> Transactions { get; set; }

        public void withdraw(decimal amount) { }

        public void deposit(decimal amount) { }


        public void generateTransaction(decimal amount, char transactionType) { }

    }
}