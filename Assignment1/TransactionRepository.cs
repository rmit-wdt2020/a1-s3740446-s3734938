using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BankingApplication
{
    public class TransactionRepository : SqlRepository, ISqlRepository<Transaction>
    {
        public void Insert(Transaction t)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [TRANSACTION] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
                                                                    " VALUES (@TransactionType, @AccountNumber, case when @DestinationAccountNumber = 0 then null else @DestinationAccountNumber end, @Amount, case when @Comment = '' then null else @Comment end, @TransactionTimeUtc)", conn);
                cmd.Parameters.AddWithValue("@TransactionType", t.TransactionType);
                cmd.Parameters.AddWithValue("@AccountNumber", t.AccountNumber);
                cmd.Parameters.AddWithValue("@DestinationAccountNumber", t.DestinationAccountNumber);
                cmd.Parameters.AddWithValue("@Amount", t.Amount);
                cmd.Parameters.AddWithValue("@Comment", t.Comment);
                cmd.Parameters.AddWithValue("@TransactionTimeUtc", t.TransactionTimeUtc);
                cmd.ExecuteNonQuery();
            }
        }
        public void Update(Transaction t)
        {
            throw new Exception("Not Implemented");
        }
        public Transaction SelectById(int id)
        {
            throw new Exception("Not Implemented");
        }
        
        //Transactions returned by account id
        public List<Transaction> SelectAll(int id)
        {
            List<Transaction> list = new List<Transaction>();
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [TRANSACTION] WHERE AccountNumber = @AccountNumber", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", id);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Transaction t = new Transaction();
                    t.TransactionId = read.GetInt32(0);
                    t.TransactionType = read.GetString(1)[0];
                    t.AccountNumber = read.GetInt32(2);
                    if (!read.IsDBNull(3))
                    {
                        t.DestinationAccountNumber = read.GetInt32(3);
                    }
                    else
                    {
                        t.DestinationAccountNumber = 0;
                    }
                    t.Amount = read.GetDecimal(4);

                    if (!read.IsDBNull(5))
                    {
                        t.Comment = read.GetString(5);
                    }
                    else
                    {
                        t.Comment = null;
                    }

                    t.TransactionTimeUtc = read.GetDateTime(6);
                    list.Add(t);

                }
                return list;

            }
        }

        public void SeedInsert(Transaction t, Account a)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand ("INSERT INTO [TRANSACTION] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
                                                                    " VALUES ('D', @AccountNumber, null, @Amount, null, @TransactionTimeUtc)", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", a.AccountNumber);
                cmd.Parameters.AddWithValue("@Amount",a.Balance);
                cmd.Parameters.AddWithValue("@TransactionTimeUtc", t.TransactionTimeUtc);
                cmd.ExecuteNonQuery();

            }
        }

    }
}
