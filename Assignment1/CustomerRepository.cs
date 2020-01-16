using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    public class CustomerRepository : SqlRepository, ISqlRepository<Customer>
    {
        public void Insert(Customer c)
        {
            using (var conn = GetConnection()){
                conn.Open();
                SqlCommand CustCmd = new SqlCommand("INSERT INTO CUSTOMER (CustomerID, Name, Address, City, Postcode)" +
                                                        " VALUES(@CustomerID, @Name, @Address, @City, @Postcode )", conn);
                CustCmd.Parameters.AddWithValue("@CustomerID", c.CustomerId);
                CustCmd.Parameters.AddWithValue("@Name", c.Name);
                CustCmd.Parameters.AddWithValue("@Address", c.Address);
                CustCmd.Parameters.AddWithValue("@City", c.City);
                CustCmd.Parameters.AddWithValue("@PostCode", c.PostCode);
                CustCmd.ExecuteNonQuery();
            }
        }
        public void Update(Customer c)
        {
            throw new Exception("Not Implemented");
        }
        public Customer SelectById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from customer where customerid = @customerId", conn);

                cmd.Parameters.AddWithValue("@customerId", id);

                read = cmd.ExecuteReader();
                Customer c = new Customer();
                while (read.Read())
                {

                    Int32.TryParse(read.GetString(1), out int cid);
                    c.CustomerId = cid;
                    c.Name = read.GetString(1);
                    c.Address = read.GetString(2);
                    c.City = read.GetString(3);
                    c.PostCode = read.GetString(4);
                }
                return c;
            }
        }
        //Not implemented for customer
        public List<Customer> SelectAll(int id)
        {
            throw new Exception("Not Implemented");

        }
    }
}
