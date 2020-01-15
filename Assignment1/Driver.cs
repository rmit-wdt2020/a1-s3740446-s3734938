using System;
using System.Threading;
using System.Linq;

namespace BankingApplication
{
    public class Driver
    {   
        // Reference to Authrepository class to call login method.
        AuthRepository auth = new AuthRepository();
        Customer customer;
        
        // Variable for keeping a tab whether a customer is logged in the system or not.
        bool customerLoggedIn = false;

        public Driver()
        {
        }

        // Initialising customer object with its accounts and transactions.
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
            // Clearing the console every time customer logs out or a new customer logs in.
            Console.Clear();

            // Taking loginID and password inputs from customer.
            Console.WriteLine("Enter your Login ID");
            string loginID = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string passWord = Console.ReadLine();
        
            // Fetching passwordhash from database to verify user identity. Result contains customerID and passwordhash.
            var result = DatabaseAccess.Instance.GetLoginDetails(loginID);
            
            // If login is successfull, initialise customer object, and redirect customer to menu to choose tasks 
            // to perform such as withdraw, deposit etc.
            if (auth.login(result.Item2, passWord))
            {
                Console.WriteLine("Login successful.Welcome.");
                customerLoggedIn = true;
                this.InitializeCustomer(result.Item1);
                this.GetCustomerChoice();
            }
            else
            {
                // If login is unsuccessfull ask the customer to enter login details again.
                Console.WriteLine("Login Failed. Please try again.");
                Thread.Sleep(3000);
                PerformLogin();
            }
        }

        public void Withdraw()
        {
            // Customer's selected account to do the withdraw.
            var account = CustomerAccountSelection();
            
            Console.WriteLine("Enter the amount you want to withdraw");
            decimal amount = 0;
            
            // Not allowing the customer to enter a negative amount and validation to check if input is a number.
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0 ) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            // Withdraw method in account class performing backend tasks.
            account.Withdraw(amount);

            Console.WriteLine("Withdraw successful");
        }

        
        public void TransferMoney()
        {
            int accountNo = 0;
            decimal amount = 0;

            // Variable to store a yes or no whether customer wants to add a comment during the transfer.
            string answer = "";

            // Varaible to store the customer's comment if anything is entered.
            string comment = "";

            // The account object from which money would be sent.
            var senderAccount = CustomerAccountSelection();
            
            Console.WriteLine("Your balance is " + senderAccount.Balance);
            
            Console.WriteLine("Type target account number for transfer: ");
            int.TryParse(Console.ReadLine(), out accountNo);

            // The account object in which money would be received.
            var receiverAccount = DatabaseAccess.Instance.GetAccountDataViaAccountID(accountNo);

            // If the receiver account object is null, it means user entered an account number to send money
            // to which does not exist. Abort transaction with error message.
            if(receiverAccount == null)
            {
                throw new InvalidDataException("Please enter a valid account number");
            }

            // Check to make sure sending and recieving accounts arent the same.
            if(receiverAccount.AccountNumber == senderAccount.AccountNumber)
            {
                throw new InvalidDataException("Sending and receiving accounts cannot be the same.");
            }

            Console.WriteLine("Please enter transfer amount");
            if (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }

            // Ask the customer to enter a comment. If customer presses y or Y the code continues asking customer to
            // input the comment which gets stored in comment variable.
            do
            {
                Console.WriteLine("Do you wish to enter a comment? (Y/N)");
                answer = Console.ReadLine();
                if(answer == "y" || answer =="Y")
                {
                    Console.WriteLine("Type your comment:");
                    comment = Console.ReadLine();
                    break;
                }
                if(answer == "n" || answer =="N")
                {
                    break;
                }
            }while(!(answer == "y" || answer == "n" || answer == "Y" || answer == "N"));
            
            // Transfer money method in account class performing the backend.
            senderAccount.TransferMoney(amount,receiverAccount,comment);
            
            Console.WriteLine("Transfer Complete");

        }

      
        public void CheckMyStatements()
        {
            var account = CustomerAccountSelection();

            Console.WriteLine("Your balance is "+account.Balance);
            Console.WriteLine("\nList of Transactions: ");
            
            // Code for paging and displaying four transactions on the screen.
            // At a time only four transactions need to be displayed. So if the transaction list has 10 items make the startIndex 9
            // ie one less than count. 
            int startIndex = account.Transactions.Count - 1;
            int endIndex = 0;
            
            // Keep decrementing startindex till it reaches a value three less than its original value. If it was 9 keep looping 
            // till it becomes 6. So we show four transactions ie at index 9,8,7,6. Ensure it doesnt fall below 0.
            for(int i = startIndex;i >= startIndex -3 && i>=0;--i)
            {
                // Suppose transactions at index 9 and 8 are being displayed on the screen, endindex would be 8 at this point
                // in time.
                endIndex = i;

                // Displaying transactions on the console.
                Transaction t = account.Transactions[i];
                string display = "TransactionType: " + t.TransactionType + " AccountNumber: " + t.AccountNumber;
                if(t.DestinationAccountNumber != 0)
                    display = display + " Destination Account Number: " + t.DestinationAccountNumber;
                display = display + " Amount: " + (double) t.Amount + " Transaction Time: " + ((DateTime) t.TransactionTimeUtc).ToLocalTime();
                if(t.Comment != null)
                    display = display + " Comment: " + t.Comment;
                Console.WriteLine(display);
                
                // Show paging options to user only if the number of transactions are more than 4. Show the paging option after the
                // fourth one has been displayed on screen. We dont want to show the paging option after every transaction.
                if(((endIndex == startIndex -3) || (endIndex ==0)) && (account.Transactions.Count() > 4))
                {
                    while(true)
                    {
                        Console.WriteLine("Type > to view next transactions, < to view previous transactions and . to return to menu");
                        string inputChar = Console.ReadLine();
                        if(inputChar == "<")
                        {
                            // If the last transaction is being shown(we started displaying from list.count-1 to 0) tell the customer there 
                            // are no more transactions to be shown.
                            if(endIndex == 0) 
                            { 
                                Console.WriteLine("No more previous transactions to display.");
                            }
                            else
                            {
                                // Suppose transactions at index 9,8,7,6 are being shown. So startindex is 9 and end is 6. Now startindex becomes 6.
                                startIndex = endIndex; 
                                // i becomes 6.
                                i = startIndex; 
                                // Startindex becomes 5. So, once we break and begin for loop its i=5 (6 becomes 5 because of the --i). 
                                // And transactions 5,4,3,2 will be shown.
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
                                // Suppose tranactions at index 5,4,3,2 are being shown. Startindex is 5. It becomes 9 to display next 4. ie 9,8,7,6.
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
            // Customer's selected account to do the deposit.
            var account = CustomerAccountSelection();

            Console.WriteLine("Enter the amount you want to deposit");
            decimal amount = 0;
            
            // Not allowing the customer to enter a negative amount and validation to check if input is a number.
            if(!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0) 
            {
                throw new InvalidDataException("Please enter a valid amount greater than 0");
            }
            
            account.Deposit(amount);
            Console.WriteLine("Deposit successfull");
        }

        // Displaying a menu of customer accounts to choose from.
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

        // Displaying a menu to the customer to select and perform a operation.
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
                    try
                    {
                        TransferMoney();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
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
                    // Exits the program.
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
