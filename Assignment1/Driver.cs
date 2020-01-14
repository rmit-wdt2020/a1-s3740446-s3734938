using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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

        public void InitializeCustomer(int customerId){
            var result = DatabaseAccess.Instance.GetCustomerDetails(customerId);
            
            customer = new Customer() {
                CustomerId = customerId, 
                Name = result.Item1,
                City = result.Item2,
                Address = result.Item3,
                PostCode = result.Item4
            };

            var accounts = DatabaseAccess.Instance.GetAccountData(customerId);

            foreach (var item in accounts) {
                
                var transactions = DatabaseAccess.Instance.GetTransactionData(item.AccountNumber);
                foreach (var transaction in transactions) {
                        item.Transactions.Add(transaction);
                    }
                customer.accounts.Add(item);
            }
        }

        public void PerformLogin()
        {   
            Thread.Sleep(3000);
            Console.Clear();
            Console.WriteLine("Enter your Login ID");
            string loginID = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string passWord = Console.ReadLine();
        
            var result = DatabaseAccess.Instance.GetLoginDetails(loginID);
            
            if (auth.login(result.Item2, passWord))
            {
                Console.WriteLine("Login successful.Welcome.");
                customerLoggedIn = true;
                this.InitializeCustomer(result.Item1);
                this.GetCustomerChoice();
            }
            else
            {
                Console.WriteLine("Login Failed. Please try again.");
                PerformLogin();
            }
        }

        public void Withdraw()
        {
            var account = CustomerAccountSelection();
            
            Console.WriteLine("Enter the amount you want to withdraw");
            decimal amount = 0;
            
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0 ) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            account.Withdraw(amount);

            Console.WriteLine("Withdraw successful");
        }

        
        public void TransferMoney()
        {
            int accountNo;
            decimal amount;
            var account = CustomerAccountSelection();
            Console.WriteLine("Your balance is " + account.Balance);
            Console.WriteLine("Type target account number for transfer: ");
            int.TryParse(Console.ReadLine(), out accountNo);
            if (DatabaseAccess.Instance.DbChk("dbo.AccountExists", accountNo) == 0)
            {
                throw new InvalidDataException("Please enter a valid account number");
            }
            Console.WriteLine("Please enter transfer amount");
            if (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }

          //  account.Withdraw(amount, 'T');
            //Incomplete transfer method, needs to update transfer target
            Console.WriteLine("Transfer Complete");

        }

      
        public void CheckMyStatements()
        {
            var account = CustomerAccountSelection();

            Console.WriteLine("Your balance is "+account.Balance);
            Console.WriteLine("\nList of Transactions: ");
            
            int startIndex = account.Transactions.Count - 1;
            int endIndex = 0;
            
            for(int i = startIndex;i >= startIndex -3 && i>=0;--i)
            {
                endIndex = i;
                Transaction t = account.Transactions[i];
                string display = "TransactionType: " + t.TransactionType + " AccountNumber: " + t.AccountNumber;
                if(t.DestinationAccountNumber != 0)
                    display = display + " Destination Account Number: " + t.DestinationAccountNumber;
                display = display + " Amount: " + (double) t.Amount + " Transaction Time: " + ((DateTime) t.TransactionTimeUtc).ToLocalTime();
                if(t.Comment != null)
                    display = display + " Comment: " + t.Comment;
                Console.WriteLine(display);
                
                if(((endIndex == startIndex -3) || (endIndex ==0)) && (account.Transactions.Count() > 4))
                {
                    while(true)
                    {
                        Console.WriteLine("Type > to view next transactions, < to view previous transactions and . to return to menu");
                        string inputChar = Console.ReadLine();
                        if(inputChar == "<")
                        {
                            if(endIndex == 0) 
                            { 
                                Console.WriteLine("No more previous transactions to display.");
                            }
                            else
                            {
                                startIndex = endIndex;
                                i = startIndex;
                                startIndex = endIndex - 1;
                                break;
                            }
                                
                        }
                        else if(inputChar == ".")
                        {
                            GetCustomerChoice();
                        }
                        else if(inputChar == ">")
                        {   
                            if(startIndex == account.Transactions.Count - 1)
                            {
                               Console.WriteLine("No further transactions to display."); 
                               
                            }
                            else
                            {
                                startIndex = startIndex + 4;
                                if(startIndex > account.Transactions.Count - 1){ startIndex = account.Transactions.Count - 1;}
                                i = startIndex + 1;
                                break;
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Please enter one of the following: '>' , '<' , '.'");
                        }
                    }
                }
            }
        }

        public void Deposit()
        {

            var account = CustomerAccountSelection();


            Console.WriteLine("Enter the amount you want to deposit");
            decimal amount = 0;
            
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            account.Deposit(amount);
            Console.WriteLine("Deposit successfull");
        }

        public Account CustomerAccountSelection()
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

        public void GetCustomerChoice()
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
                        Withdraw();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    case "2":
                    try
                    {
                        Deposit();
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
                        CheckMyStatements();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                    case "5":
                    PerformLogin();
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
