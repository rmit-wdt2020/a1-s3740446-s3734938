using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    //Account Repository containing SQL
    public class AccountRepository : SqlRepository, ISqlRepository<Account>
    {
        public void Insert(Account a)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand AccCmd = new SqlCommand("INSERT INTO ACCOUNT (AccountNumber, AccountType, CustomerID, Balance)" +
                                                           " VALUES (@AccountNumber, @AccountType, @CustomerID, @Balance)", conn);
                AccCmd.Parameters.AddWithValue("@AccountNumber", a.AccountNumber);
                AccCmd.Parameters.AddWithValue("@AccountType", a.GetType().Name[0]);
                AccCmd.Parameters.AddWithValue("@CustomerID", a.CustomerId);
                AccCmd.Parameters.AddWithValue("@Balance", a.Balance);
                AccCmd.ExecuteNonQuery();
            }
        }
        public void Update(Account a)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand AccCmd = new SqlCommand("UPDATE ACCOUNT SET AccountNumber = @AccountNumber, AccountType = @AccountType," +
                    " CustomerID = @CustomerID, Balance = @Balance WHERE AccountNumber = @AccountNumber", conn);
                AccCmd.Parameters.AddWithValue("@AccountNumber", a.AccountNumber);
                AccCmd.Parameters.AddWithValue("@AccountType", a.GetType().Name[0]);
                AccCmd.Parameters.AddWithValue("@CustomerID", a.CustomerId);
                AccCmd.Parameters.AddWithValue("@Balance", a.Balance);
                AccCmd.ExecuteNonQuery();
            }

        }
        public Account SelectById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand AccCmd = new SqlCommand("SELECT * FROM ACCOUNT WHERE AccountNumber = @AccountNumber", conn);
                AccCmd.Parameters.AddWithValue("@AccountNumber", id);
                AccCmd.ExecuteNonQuery();

                read = AccCmd.ExecuteReader();
                Account a = null;
                while (read.Read())
                {
                   a = AccountFactory.CreateAccount(read.GetInt32(0), read.GetString(1)[0], read.GetInt32(2), read.GetDecimal(3));
                }
                return a;
            }
        }
        public List<Account> SelectAll(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                List<Account> list = new List<Account>();
                SqlCommand AccCmd = new SqlCommand("SELECT * FROM ACCOUNT WHERE CustomerID = @CustomerID", conn);
                AccCmd.Parameters.AddWithValue("@CustomerID", id);
                AccCmd.ExecuteNonQuery();

                read = AccCmd.ExecuteReader();
                while (read.Read())
                {
                    Account a = AccountFactory.CreateAccount(read.GetInt32(0), read.GetString(1)[0], read.GetInt32(2), read.GetDecimal(3));
                    list.Add(a);

                }
                return list;
        }

    }
    }
}
