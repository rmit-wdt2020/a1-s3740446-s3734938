using System;
using SimpleHashing;
using Microsoft.Data.SqlClient;

namespace HelloWorldApplication
{
    public class AuthRepository
    {
        //string hash = PBKDF2.Hash("abc123");
        string hash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV + I8d2";

        public AuthRepository()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "wdt2020.australiasoutheast.cloudapp.azure.com";
            builder.UserID = "s3734938";
            builder.Password = "abc123";
            builder.InitialCatalog = "s3734938";

            //SqlConnection connection = new SqlConnection(builder.ConnectionString);

            //string connectionString = "server=wdt2020.australiasoutheast.cloudapp.azure.com;uid=s3734938;database=s3734938;pwd=abc123";
            //SqlConnection con = new SqlConnection(connectionString);
           // SqlCommand cmd = new SqlCommand("Select * from Customer", connection);
        }

        public bool login(string loginId, string password)
        {
            bool userValidated = false;
                
            userValidated = PBKDF2.Verify(hash, password);

            return userValidated;

            
        }
    }
}
