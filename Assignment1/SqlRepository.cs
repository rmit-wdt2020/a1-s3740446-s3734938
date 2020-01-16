using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    //Abstract repository for handling connection logic for SQL repositories
    public abstract class SqlRepository
    {
        //Connection string retrieval from Json.
        //Referencing Web Development Technologies lectures and tutorials
        private IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        protected SqlDataReader read;
        public SqlConnection GetConnection()
        {
            return new SqlConnection(Configuration["ConnectionString"]);
        }
    }
}
