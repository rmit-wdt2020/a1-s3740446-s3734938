using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public class Driver
    {
        AuthRepository auth = new AuthRepository();
        Customer customer;
         bool customerLoggedIn = false;

        public Driver()
        {
        }

        public void initializeCustomer(int customerId){
            var result = DatabaseAccess.Instance.getCustomerDetails(customerId);
            
            customer = new Customer() {
                CustomerId = customerId, 
                Name = result.Item1,
                City = result.Item2,
                Address = result.Item3,
                PostCode = result.Item4
            };

            var accounts = DatabaseAccess.Instance.getAccountData(customerId);

            foreach (var item in accounts) {
                customer.accounts.Add(item);
            }
        }

        public void performLogin()
        {
            Console.Clear();
            Console.WriteLine("Enter your Login ID");
           string loginID = Console.ReadLine();
        //    string loginID = "12345678";
            Console.WriteLine("Enter your password");
            string passWord = Console.ReadLine();
        //    string passWord = "abc123";
            
            var result = DatabaseAccess.Instance.getLoginDetails(loginID);
            
            if (auth.login(result.Item2, passWord))
            {
                Console.WriteLine("Login successful.Welcome.");
                customerLoggedIn = true;
                this.initializeCustomer(result.Item1);
                this.getCustomerChoice();
            }
            else
            {
                Console.WriteLine("Login Failed. Please try again.");
                performLogin();
            }
        }

         public void withdraw(){
             Console.WriteLine("Enter the account number");
             int accountNumber = 0;
             if(!int.TryParse(Console.ReadLine(), out accountNumber) || accountNumber <= 0) {
                Console.WriteLine("You entered invalid data.Please try again");
                return;
            }
             Console.WriteLine("Enter the amount you want to withdraw");
             decimal amount = 0;
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0) {
                Console.WriteLine("You entered invalid data.Please try again");
                return;
            }
            var account = customer.accounts.Find(a => a.AccountNumber == accountNumber);
            if(account == null) {
                    Console.WriteLine("Account number does not exist.Please try again");
                    return;
                }
            try
            {
                account.withdraw(amount);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
         }

         public bool checkIfAccountExists(int accountNumber){
            var account = customer.accounts.Find(a => a.AccountNumber == accountNumber);
            if(account == null) {
                return false;
            }
            else{
                return true;
            }
         }

         public void deposit(){
             Console.WriteLine("Enter the account number");
             int accountNumber = 0;
             
            if(!int.TryParse(Console.ReadLine(), out accountNumber) || accountNumber <= 0) {
                Console.WriteLine("You entered an invalid account number.Please try again");
                return;
            }

            var account = customer.accounts.Find(a => a.AccountNumber == accountNumber);
            if(account == null) {
                Console.WriteLine("Account number does not exist.Please try again");
                return;
            }

             Console.WriteLine("Enter the amount you want to deposit");
             decimal amount = 0;
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0) {
                Console.WriteLine("You entered invalid data.Please try again");
                return;
            }
           
           
            try
            {
                account.deposit(amount);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
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
    
                string option = Console.ReadLine();
               //  string option = "2";
                switch (option)
                {
                    case "1":
                        Console.WriteLine("Withdraw");
                        withdraw();
                        break;
                    case "2":
                        Console.WriteLine("Deposit");
                        deposit();
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
