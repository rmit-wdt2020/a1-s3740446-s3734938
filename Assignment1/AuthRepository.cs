using SimpleHashing;

namespace BankingApplication
{
    public class AuthRepository
    {
        public AuthRepository()
        {
        }

        // Uses the static method verify in the PBKDF2 static class located in the simple hashing namespace
        // to verify user inputted password with stored password hash.
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
