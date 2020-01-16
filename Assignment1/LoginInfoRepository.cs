using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class LoginInfoRepository : SqlRepository, ISqlRepository<LoginInfo>
    {
        public void Insert(LoginInfo l)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Login (LoginID, CustomerID, PasswordHash)" +
                    " VALUES(@LoginID, @CustomerID, @PasswordHash)", conn);
                cmd.Parameters.AddWithValue("@LoginID", l.LoginId);
                cmd.Parameters.AddWithValue("@CustomerID", l.CustomerId);
                cmd.Parameters.AddWithValue("@PasswordHash", l.PasswordHash);
                cmd.ExecuteNonQuery();
            }
        }
        public void Update(LoginInfo l)
        {
            throw new Exception("Not Implemented");
        }
        public LoginInfo SelectById(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM LOGIN WHERE loginid = @loginid", conn);
                cmd.Parameters.AddWithValue("@loginid", id);

                read = cmd.ExecuteReader();
                LoginInfo l = null;
                while (read.Read())
                {
                    l = new LoginInfo()
                    {
                        LoginId = read.GetString(0),
                        CustomerId = read.GetInt32(1),
                        PasswordHash = read.GetString(2)
                    };
                }
                return l;
            }
        }
        public List<LoginInfo> SelectAll(int id)
        {
            throw new Exception("Not Implemented");
        }
    }
}
