using System;
namespace HelloWorldApplication
{
    public class Customer
    {
        private int customerId;
        private string name;
        private string address;
        private string city;
        private string postCode;

        public int CustomerId   
        {
            get { return customerId; }   
        }

        public string Name
        {
            get { return name; }
        }

        public string Address
        {
            get { return address; }
        }

        public string City
        {
            get { return city; }
        }

        public string PostCode
        {
            get { return postCode; }
        }

        public Customer()
        {
        }
    }
}

    //CustomerID int not null,
    //Name nvarchar(50) not null,
    //Address nvarchar(50) null,
    //City nvarchar(40) null,
    //PostCode nvarchar(4) null,
    //constraint PK_Customer primary key(CustomerID)