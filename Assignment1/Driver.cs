using System;
namespace HelloWorldApplication
{
    public class Driver
    {
        AuthRepository auth = new AuthRepository();
        Customer customer;

        public Driver()
        {
        }

        public void performLogin()
        {
            Console.WriteLine("Enter your Login ID");
            string loginID = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string passWord = Console.ReadLine();

            if (auth.login(loginID, passWord))
            {
                Console.WriteLine("Login successful");
            }
            else
            {
                Console.WriteLine("Login Failed. Please try again.");
                performLogin();
            }
        }
    }
}
