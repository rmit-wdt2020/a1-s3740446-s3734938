using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HelloWorldApplication
{
    public class DatabaseAccess
    {

        public static void establishConnection()
        {
            SqlConnection conn = new SqlConnection ("server=wdt2020.australiasoutheast.cloudapp.azure.com;uid=s3734938;database=s3734938;pwd=abc123;");
            SqlCommand query = new SqlCommand ("select * from customer", conn);
            SqlDataReader read; 

            try
            {
                conn.Open();

                read = query.ExecuteReader();

                while (read.Read())
                {
                Console.WriteLine ("{0}\n{1}\n",
                                    read["name"],
                                    read["address"]);
                }
                
                read.Close();
            }
            catch (SqlException se)
            {
                Console.WriteLine ("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine ("Exception: {0}", e.Message);
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