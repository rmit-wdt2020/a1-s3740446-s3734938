using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HelloWorldApplication
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
        SqlConnection conn = new SqlConnection ("server=wdt2020.australiasoutheast.cloudapp.azure.com;uid=s3734938;database=s3734938;pwd=abc123;");
        SqlCommand query;
        SqlDataReader read; 
        public string getPasswordHash(string loginId)
        {       
            string hash = "";
            try
            {
                conn.Open();
                query = new SqlCommand ("select * from login where loginid = '" + loginId + "'", conn);
                read = query.ExecuteReader();

                while (read.Read())
                 {
                     hash = (string) read["passwordhash"];
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
            return hash;
        }
        
      
    }
}