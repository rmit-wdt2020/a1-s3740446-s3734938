using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    public abstract class SqlRepository
    {
        private IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        protected SqlDataReader read;
        public SqlConnection GetConnection()
        {
            return new SqlConnection(Configuration["ConnectionString"]);
        }
    }
}
