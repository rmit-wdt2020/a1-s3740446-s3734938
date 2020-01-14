using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public interface IAccount
    {
        int AccountNumber { get; set; }
        int CustomerId { get; set; }
        decimal Balance { get; set; }
        List<Transaction> Transactions { get; set; }

        public void Withdraw(decimal amount, char type = 'W') { }

        public void Deposit(decimal amount) { }


        public void GenerateTransaction(decimal amount, char transactionType) { }

    }
}