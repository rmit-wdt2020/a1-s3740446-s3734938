using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace BankingApplication
{
    public class DatabaseAccess
    {
        static readonly HttpClient Client = new HttpClient();
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
        SqlConnection conn = new SqlConnection ("server=wdt2020.australiasoutheast.cloudapp.azure.com;uid=s3740446;database=s3740446;pwd=abc123;");
        SqlCommand query;
        SqlDataReader read;
        
        public int DbChk()
        {
            SqlCommand cmd = new SqlCommand("dbo.CheckDb", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            //Output Parameter
            cmd.Parameters.Add("@bool", SqlDbType.Bit).Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                int chkresponse = Convert.ToInt32(cmd.Parameters["@bool"].Value);
                return chkresponse;
            }
                catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
                return 3;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                return 3;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }
        }
        

        public async Task GetJson()
        {
            Console.WriteLine("Getting Json");
            var cjson = await Client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");
            var ljson = await Client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");

            List<Customer> tmpList = JsonConvert.DeserializeObject<List<Customer>>(cjson);

            foreach (Customer c in tmpList)
            {
                foreach(Account a in c.Accounts)
                {
                    a.InitTransaction();
                }
            }

            SqlCommand LoginCmd = new SqlCommand("dbo.InsertLogin", conn);
            LoginCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter jsonparam = new SqlParameter("@json", ljson);
            LoginCmd.Parameters.Add(jsonparam);

            try
            {
                conn.Open();
                foreach (Customer c in tmpList)
                {
                    SqlCommand CustCmd = new SqlCommand("INSERT INTO CUSTOMER (CustomerID, Name, Address, City, Postcode)" +
                                                        " VALUES(@CustomerID, @Name, @Address, @City, @Postcode )", conn);
                    CustCmd.Parameters.AddWithValue("@CustomerID", c.CustomerId);
                    CustCmd.Parameters.AddWithValue("@Name", c.Name);
                    CustCmd.Parameters.AddWithValue("@Address", c.Address);
                    CustCmd.Parameters.AddWithValue("@City", c.City);
                    CustCmd.Parameters.AddWithValue("@PostCode", c.PostCode);
                    CustCmd.ExecuteNonQuery();
                    foreach (Account a in c.Accounts)
                    {
                        SqlCommand AccCmd = new SqlCommand("INSERT INTO ACCOUNT (AccountNumber, AccountType, CustomerID, Balance)" +
                                                           " VALUES (@AccountNumber, @AccountType, @CustomerID, @Balance)", conn);
                        AccCmd.Parameters.AddWithValue("@AccountNumber", a.AccountNumber);
                        AccCmd.Parameters.AddWithValue("@AccountType", a.AccountType);
                        AccCmd.Parameters.AddWithValue("@CustomerID", a.CustomerId);
                        AccCmd.Parameters.AddWithValue("@Balance", a.Balance);
                        AccCmd.ExecuteNonQuery();
                        foreach(Transaction t in a.Transactions)
                        {
                            SqlCommand TranCmd = new SqlCommand("INSERT INTO [TRANSACTION] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, TransactionTimeUtc)" +
                                                                " VALUES (@TransactionType, @AccountNumber, @DestinationAccountNumber, @Amount, @TransactionTimeUtc)", conn);
                            TranCmd.Parameters.AddWithValue("@TransactionType", t.TransactionType);
                            TranCmd.Parameters.AddWithValue("@AccountNumber", t.AccountNumber);
                            TranCmd.Parameters.AddWithValue("@DestinationAccountNumber", t.DestinationAccountNumber);
                            TranCmd.Parameters.AddWithValue("@Amount", t.Amount);
                            TranCmd.Parameters.AddWithValue("@TransactionTimeUtc", t.TransactionTimeUtc);
                            TranCmd.ExecuteNonQuery();
                        }
                    }
                }
                LoginCmd.ExecuteNonQuery();
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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }

        }

        public void updateBalance(decimal amount, int accountNumber)
                {
                    try
                    {
                        conn.Open();

                        // 1. declare command object with parameter
                        SqlCommand cmd = new SqlCommand("update account set balance = @balance where accountnumber = @accountNumber", conn);

                        cmd.Parameters.AddWithValue("@balance",amount);
                        cmd.Parameters.AddWithValue("@accountNumber",accountNumber);

                        // get data stream
                        int update = cmd.ExecuteNonQuery();

                    }
                    finally
                    {
                        // close reader
                        if (read != null)
                        {
                            read.Close();
                        }

                        // close connection
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
                    finally
                    {
                        // close reader
                        if (read != null)
                        {
                            read.Close();
                        }

                        // close connection
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

                // 1. declare command object with parameter
                SqlCommand cmd = new SqlCommand("select * from login where loginid = @loginid", conn);

                cmd.Parameters.AddWithValue("@loginid",loginId);

                // get data stream
                read = cmd.ExecuteReader();

                // write each record
                while(read.Read())
                {
                    // Console.WriteLine("{0}", 
                    //     read["passwordhash"]);
                    customerId = read.GetInt32(1);
                    passwordhash = read.GetString(2);
                }
            }
            finally
            {
                // close reader
                if (read != null)
                {
                    read.Close();
                }

                // close connection
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
                query = new SqlCommand("select * from account where customerid = '" + customerId + "'", conn);
                read = query.ExecuteReader();

                while (read.Read())
                {
                    accountNumber = read.GetInt32("accountnumber");
                    accountType = read.GetString(1)[0];
                    balance = read.GetDecimal(3);

                    Account account = new Account() {
                        AccountNumber = accountNumber,
                        Balance = balance
                    };
                    if(accountType == 'S'){
                        //account.AccountType = Type.Savings;
                    }
                    else{
                        //account.AccountType = Type.Checking;
                    }
                    accounts.Add(account);
                }

                read.Close();


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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }
            
            return accounts;
        }
        
      
    }
}