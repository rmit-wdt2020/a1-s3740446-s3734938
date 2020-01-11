using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public class Customer
    {
        private int customerId;
        private string name;
        private string address;
        private string city;
        private string postCode;
        public List<Account> accounts = new List<Account>(); 

        public int CustomerId   
        {
            get { return customerId; }
            set { customerId = value; }   
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public List<Account> Accounts
        {
            get { return accounts; }
            set { }
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