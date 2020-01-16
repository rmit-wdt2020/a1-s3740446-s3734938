using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BankingApplication
{
    public class DatabaseAccess
    {
        static readonly HttpClient Client = new HttpClient();

        //Repository Objects
        private CustomerRepository CustomerRepo = new CustomerRepository();
        private AccountRepository AccountRepo = new AccountRepository();
        private TransactionRepository TransactionRepo = new TransactionRepository();
        private LoginInfoRepository LoginInfoRepo = new LoginInfoRepository();
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
        
          
        
        public int DbChk(string sproc, int? account = null)
        {
            SqlCommand cmd = new SqlCommand(sproc, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            //Output Parameter
            cmd.Parameters.Add("@bool", SqlDbType.Bit).Direction = ParameterDirection.Output;

            if (account.HasValue)
            {
                cmd.Parameters.AddWithValue("@accountNo", account);
            }

            
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
            var cjson = await Client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");
            var ljson = await Client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");

            //Variable for setting datetime format for reading json
            var format = "dd/MM/yyyy hh:mm:ss tt";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            AccountConverter converter = new AccountConverter();

            //Deserialize json into list (Referenced from Web Development Tutorial 2 but with added date time converter)
            List<Customer> tmpList = JsonConvert.DeserializeObject<List<Customer>>(cjson, converter, dateTimeConverter);
            List<LoginInfo> ltmpList = JsonConvert.DeserializeObject<List<LoginInfo>>(ljson);

            try
            {
                conn.Open();
                foreach (Customer c in tmpList)
                {
                    CustomerRepo.Insert(c);
                    foreach (Account a in c.Accounts)
                    {
                        AccountRepo.Insert(a);
                        foreach(Transaction t in a.Transactions)
                        {
                            TransactionRepo.SeedInsert(t, a);
                        }
                    }
                }
                foreach (LoginInfo l in ltmpList)
                {
                    LoginInfoRepo.Insert(l);
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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
            }
        }
    }
}