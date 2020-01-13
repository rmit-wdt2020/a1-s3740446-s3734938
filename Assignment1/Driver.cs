using System;
using System.Collections.Generic;
using System.Threading;

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
                
                var transactions = DatabaseAccess.Instance.getTransactionData(item.AccountNumber);
                foreach (var transaction in transactions) {
                        item.Transactions.Add(transaction);
                    }
                customer.accounts.Add(item);
            }
        }

        public void performLogin()
        {   
            Thread.Sleep(3000);
            Console.Clear();
            Console.WriteLine("Enter your Login ID");
            string loginID = Console.ReadLine(); 
            Console.WriteLine("Enter your password");
            string passWord = Console.ReadLine();
        
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

        public void withdraw()
        {
            var account = CustomerAccountSelection();

            Console.WriteLine("Enter the amount you want to withdraw");
            decimal amount = 0;
            
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0 ) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            account.withdraw(amount);
            Console.WriteLine("Withdraw successfull");
        }

        public void TransferMoney()
        {
            var account = CustomerAccountSelection();
            Console.WriteLine("Your balance is " + account.Balance);

        }

        public void checkMyStatements()
        {
            var account = CustomerAccountSelection();

            Console.WriteLine("Your balance is "+account.Balance);
            Console.WriteLine("\nList of Transactions: ");
            for(int i = account.Transactions.Count -1;i >= account.Transactions.Count -4 && i>=0;i--)
            {
                Transaction t = account.Transactions[i];
                string display = "TransactionType: " + t.TransactionType + " AccountNumber: " + t.AccountNumber;
                if(t.DestinationAccountNumber != 0)
                    display = display + " Destination Account Number: " + t.DestinationAccountNumber;
                display = display + " Amount: " + t.Amount + " Transaction Time: " + ((DateTime) t.TransactionTimeUtc).ToLocalTime();
                if(t.Comment != null)
                    display = display + " Comment: " + t.Comment;
                Console.WriteLine(display);
            }
        }

        public void deposit()
        {
            var account = CustomerAccountSelection();

            Console.WriteLine("Enter the amount you want to deposit");
            decimal amount = 0;
            
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            account.deposit(amount);
            Console.WriteLine("Deposit successfull");
        }

        //Method needs to changing to find account outside of customer account
        public IAccount checkIfAccountExists(int accountNumber)
        {
            var account = customer.accounts.Find(a => a.AccountNumber == accountNumber);
            if(account == null) 
            {
                throw new Exception("Account does not exist.");
            }
            return account;
        }

        public IAccount CustomerAccountSelection()
        {
            var count = 1;
            var selection = 0;
            foreach(var account in customer.Accounts)
            {
                Console.WriteLine(count + ": " +account.AccountNumber);
                count++;
            }
            if (!int.TryParse(Console.ReadLine(), out selection) || selection < 1 || selection > customer.Accounts.Count)
            {
                throw new InvalidDataException("Please enter a valid number");
            }

            return customer.Accounts[selection - 1];
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
                
                switch (option)
                {
                    case "1":
                    try
                    {
                        withdraw();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    case "2":
                    try
                    {
                        deposit();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    case "3":
                    break;
                    case "4":
                    try
                    {
                        checkMyStatements();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    case "5":
                    performLogin();
                    break;
                    case "6":
                    Environment.Exit(0);
                    break;
                    default:
                    Console.WriteLine("Please enter a number between 1 and 6");
                    break;
                        
                }
            }       
        }    
    }
}
