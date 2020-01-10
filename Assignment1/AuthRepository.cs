using System;
using SimpleHashing;
using Microsoft.Data.SqlClient;

namespace HelloWorldApplication
{
    public class AuthRepository
    {
        public AuthRepository()
        {
        }

        public bool login(string hash, string password)
        {
            bool userValidated = false;
            
            if(hash != null && hash!="")
            {
                userValidated = PBKDF2.Verify(hash, password);
            }

            return userValidated;
        }
        
    }
}
