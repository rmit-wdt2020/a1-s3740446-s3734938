using System;
namespace HelloWorldApplication
{
    public class Driver
    {
        AuthRepository auth = new AuthRepository();
        Customer customer;
         bool customerLoggedIn = false;

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
                Console.WriteLine("Login successful.Welcome.");
                customerLoggedIn = true;
                getCustomerChoice();
            }
            else
            {
                Console.WriteLine("Login Failed. Please try again.");
                performLogin();
            }
        }

        public void getCustomerChoice()
        {
            while(customerLoggedIn)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1) Withdraw money");
                Console.WriteLine("2) Deposit money");
                Console.WriteLine("3) Transfer money");
                Console.WriteLine("4) My statements");
                Console.WriteLine("5) Logout");
                Console.WriteLine("6) Exit");
                Console.Write("\r\nSelect an option: ");
    
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Withdraw");
                        break;
                    case "2":
                        Console.WriteLine("Deposit");
                        break;
                    case "3":
                    Console.WriteLine("Transfer");
                        break;
                    case "4":
                    Console.WriteLine("My statements");
                        break;
                    case "5":
                    Console.WriteLine("Logout");
                    performLogin();
                        break;
                    case "6":
                    Console.WriteLine("Exit");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Enter a number between 1 and 6");
                        break;
                
                }
            }
            
        }    
    }
}
