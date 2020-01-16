using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication
{
    class LoginInfo
    {
        private string loginid;
        private int customerid;
        private string passwordhash;

        public string LoginId { get; set; }
        public int CustomerId { get; set; }
        public string PasswordHash { get; set; }
    }
}
