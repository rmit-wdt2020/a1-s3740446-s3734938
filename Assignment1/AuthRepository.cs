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

        public bool login(string loginId, string password)
        {
            bool userValidated = false;
            
            string hash = DatabaseAccess.Instance.getPasswordHash(loginId); 
            
            if(hash != null && hash!="")
            {
                userValidated = PBKDF2.Verify(hash, password);
            }

            return userValidated;
        }
    }
}
