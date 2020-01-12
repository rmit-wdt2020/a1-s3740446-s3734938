using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BankingApplication
{
    public class DatabaseAccess
    {    
        private DatabaseAccess()  
        {  
        }  
        private static DatabaseAccess instance = null;  
        public static DatabaseAccess Instance  
        {  
            get  
            {  
                if (instance == null)  
                {
                    instance = new DatabaseAccess();  
                }  
                return instance;  
            }  
        } 

        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); 

        private static string ConnectionString { get; } = Configuration["ConnectionString"];
        private static SqlConnection conn = new SqlConnection (ConnectionString);
        private SqlDataReader read; 

        public void updateBalance(decimal amount, int accountNumber)
                {
                    try
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("update account set balance = @balance where accountnumber = @accountNumber", conn);

                        cmd.Parameters.AddWithValue("@balance",amount);
                        cmd.Parameters.AddWithValue("@accountNumber",accountNumber);

                        int update = cmd.ExecuteNonQuery();

                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("SQL Exception: {0}", se.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                    finally
                    {
                        if (read != null)
                        {
                            read.Close();
                        }

                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }
                }

        public void insertTransaction(Transaction t)
                {
                    try
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("INSERT INTO [TRANSACTION] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, TransactionTimeUtc)" +
                                                                " VALUES (@TransactionType, @AccountNumber, case when @DestinationAccountNumber = 0 then null else @DestinationAccountNumber end, @Amount, @TransactionTimeUtc)", conn);
                        cmd.Parameters.AddWithValue("@TransactionType", t.TransactionType);
                        cmd.Parameters.AddWithValue("@AccountNumber", t.AccountNumber);
                        cmd.Parameters.AddWithValue("@DestinationAccountNumber", t.DestinationAccountNumber);
                        cmd.Parameters.AddWithValue("@Amount", t.Amount);
                        cmd.Parameters.AddWithValue("@TransactionTimeUtc", t.TransactionTimeUtc);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("SQL Exception: {0}", se.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                    finally
                    {
                        if (read != null)
                        {
                            read.Close();
                        }

                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }
                }
        public (string,string,string,string) getCustomerDetails(int customerId)
                {
                    string name = "";
                    string address = "";
                    string city = "";
                    string postcode = "";
                    try
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("select * from customer where customerid = @customerId", conn);

                        cmd.Parameters.AddWithValue("@customerId",customerId);

                        read = cmd.ExecuteReader();

                        while(read.Read())
                        {
                            name = read.GetString(1);
                            address = read.GetString(2);
                            city = read.GetString(3);
                            postcode = read.GetString(4);
                        }
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("SQL Exception: {0}", se.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                    finally
                    {
                        if (read != null)
                        {
                            read.Close();
                        }

                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }
                    return (name,address,city,postcode);
                }
        public (int,string) getLoginDetails(string loginId)
        {
            string passwordhash = "";
            int customerId = 0;
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from login where loginid = @loginid", conn);

                cmd.Parameters.AddWithValue("@loginid",loginId);

                read = cmd.ExecuteReader();

                while(read.Read())
                {
                    customerId = read.GetInt32(1);
                    passwordhash = read.GetString(2);
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (read != null)
                {
                    read.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
            return (customerId,passwordhash);
        }

        public List<Account> getAccountData(int customerId)
        {       
            int accountNumber = 0;
            decimal balance = 0;
            char accountType = 'q';
            List<Account> accounts = new List<Account>(); 
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from account where customerid = @customerid", conn);

                cmd.Parameters.AddWithValue("@customerid",customerId);

                read = cmd.ExecuteReader();

                while (read.Read())
                {
                    accountNumber = read.GetInt32("accountnumber");
                    accountType = read.GetString(1)[0];
                    balance = read.GetDecimal(3);

                    Account account = new Account() {
                        AccountNumber = accountNumber,
                        Balance = balance,
                        AccountType = accountType
                    };
                    
                    accounts.Add(account);
                }

            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (read != null)
                {
                    read.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
            
            return accounts;
        }

        public List<Transaction> getTransactionData(int accountId)
        {       
            int transactionId;
            char transactionType;
            int accountNumber;
            int destinationAccountNumber;
            decimal amount;
            string comment;
            DateTime transactionTimeUtc; 
            List<Transaction> transactions = new List<Transaction>(); 
            try
            {
               conn.Open();
                SqlCommand cmd = new SqlCommand("select * from [transaction] where accountnumber = @accountId", conn);

                cmd.Parameters.AddWithValue("@accountid",accountId);

                read = cmd.ExecuteReader();

                while (read.Read())
                {
                    transactionId = read.GetInt32("transactionId");
                    accountNumber = read.GetInt32("accountNumber");
                    
                    if (!read.IsDBNull(3))
                        destinationAccountNumber = read.GetInt32("destinationAccountNumber");
                    else
                        destinationAccountNumber = 0;

                    transactionType = read.GetString(1)[0];
                    amount = read.GetDecimal(4);
                    
                    if (!read.IsDBNull(5))
                        comment = read.GetString(5);
                    else
                        comment = null;

                    transactionTimeUtc = read.GetDateTime(6);

                    Transaction transaction = new Transaction(){
                        TransactionId = transactionId,
                        TransactionType = transactionType,
                        AccountNumber = accountNumber,
                        Amount = amount,
                        Comment = comment,
                        DestinationAccountNumber = destinationAccountNumber,
                        TransactionTimeUtc = transactionTimeUtc
                    };
                    
                    transactions.Add(transaction);
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (read != null)
                {
                    read.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
            
            return transactions;
        }
    }
}