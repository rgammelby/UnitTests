using Xunit;
using UnitTests.Bank;
using UnitTests.Customers;
using UnitTests.Factories;
using UnitTests.Controllers;
using UnitTests.Repositories;
using UnitTests.Services;

namespace UnitTests.Tests.Tests
{
    public class Tests
    {
        /// <summary>
        /// Initializes factories for uniform customer and bank account creation.
        /// </summary>
        private CustomerFactory _customerFactory;
        private BankAccountFactory _bankAccountFactory;

        public Tests()
        {
            _customerFactory = new CustomerFactory();
            _bankAccountFactory = new BankAccountFactory();
        }

        /// <summary>
        /// This test checks that user creation is possible using the CustomerFactory.
        /// </summary>
        [Fact]
        public void T_Can_Create_Customer_With_Factory()
        {
            // Customer Factory returns a new user
            Customer c = _customerFactory.Create();

            // Ensure that the returned user is not null
            Assert.NotNull(c);
        }

        /// <summary>
        /// This test checks that bank account creation is possible.
        /// </summary>
        [Fact]
        public void T_Can_Create_Bank_Account_With_Factory()
        {
            // BankAccount factory returns a new bank account
            BankAccount a = _bankAccountFactory.Create();

            // Ensure that the returned bank account is not null
            Assert.NotNull(a);
        }

        /// <summary>
        /// This test checks that it is possible to add a bank account to a customer's list of accounts. 
        /// </summary>
        [Fact]
        public void T_Can_Add_Bank_Account_To_User()
        {
            // Return a new customer
            Customer c = _customerFactory.Create();

            // Return a new bank account
            BankAccount account = _bankAccountFactory.Create();

            // Add the bank account to the customer's list of accounts
            c.AddAccount(account);
            Assert.Single(c.accounts);
        }

        /// <summary>
        /// This test checks that it is possible for a user to own several bank accounts (no upper limit).
        /// </summary>
        [Fact]
        public void T_Can_Add_Multiple_Bank_Accounts_To_User()
        {
            // Return a customer
            Customer c = _customerFactory.Create();

            // Return 3 new, unique bank accounts
            BankAccount firstAccount = _bankAccountFactory.Create();
            BankAccount secondAccount = _bankAccountFactory.Create();
            BankAccount thirdAccount = _bankAccountFactory.Create();

            // Add all three bank accounts to the same customer's list of accounts
            c.AddAccount(firstAccount);
            c.AddAccount(secondAccount);
            c.AddAccount(thirdAccount);

            // Ensure that all accounts have been added to the user
            Assert.Equal(3, c.accounts.Count);
        }

        /// <summary>
        /// This test checks that a customer is able to deposit money into an account belonging to them. 
        /// </summary>
        [Fact]
        public void T_User_Can_Deposit_Money()
        {
            // Defining deposited amount and expected account balance
            const int DEPOSIT_AMOUNT = 5000;
            const int EXPECTED_ACCOUNT_BALANCE = 15000;

            // Return a new customer and bank account
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Adds bank account to customer's list of accounts
            c.AddAccount(account);

            // Define new variable for specific customer account
            BankAccount customerAccount = c.accounts.First();

            // Deposit into specific customer account
            customerAccount.Deposit(DEPOSIT_AMOUNT);

            // Ensure that the specific customer's account balance matches the expected balance
            Assert.Equal(EXPECTED_ACCOUNT_BALANCE, customerAccount.Balance);
        }

        /// <summary>
        /// This test checks that the customer is not able to withdraw more money than exists in their account. No money will be withdrawn if this operation is attempted.
        /// </summary>
        [Fact]
        public void T_User_Cannot_Withdraw_Money_Beyond_Their_Balance()
        {
            // Defining overdraw amount and expected account balance at the end
            const int EXPECTED_ACCOUNT_BALANCE = 10000;
            const int OVERDRAW_AMOUNT = 15000;

            // Return new user and bank account
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Add bank account to customer's list of accounts
            c.AddAccount(account);
            BankAccount customerAccount = c.accounts.First();

            // Saves the expected exception for this invalid operation (drawing an amount larger than the account's balance)
            var exception = Assert.Throws<InvalidOperationException>(() => customerAccount.Withdraw(OVERDRAW_AMOUNT));
            Assert.Equal("Insufficient funds. ", exception.Message);

            // Ensures that no funds have been withdrawn by checking the customer's account balance against the expected account balance (it remains unchanged)
            Assert.True(customerAccount.GetBalance() == EXPECTED_ACCOUNT_BALANCE);
        }

        /// <summary>
        /// This test checks that the user is able to withdraw an amount of money which is smaller than the current balance in their account.
        /// </summary>
        [Fact]
        public void T_User_Can_Withdraw_Money_Within_Their_Balance()
        {
            // Defining the amount to be withdrawn
            const int WITHDRAW_AMOUNT = 5000;

            // Returns a new customer and bank account
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Defines initial balance, which is equal to the default balance found in the bank account's constructor
            int initialBalance = account.Balance;

            // Adds the account to the customer's list of accounts
            c.AddAccount(account);

            // Sets a variable to ensure the customer's specific account is used    
            BankAccount customerAccount = c.accounts.First();

            // Attempts to withdraw a "legal" amount of money from the user's account
            customerAccount.Withdraw(WITHDRAW_AMOUNT);

            // Ensures that the account's balance matches the expected output, which is the initial balance minus the withdrawn amount
            Assert.True(customerAccount.GetBalance() == initialBalance - WITHDRAW_AMOUNT);
        }

        /// <summary>
        /// This test checks whether the user is able to perform a withdrawal operaton of NO money.
        /// </summary>
        [Fact]
        public void T_User_Cannot_Withdraw_No_Money()
        {
            // Defines withdraw amount as zero
            const int WITHDRAW_AMOUNT = 0;

            // Returns new customer and bank account
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Defines initial balance, which is equal to the default balance found in the bank account's constructor
            int initialBalance = account.Balance;

            // Adds the returned account to the customer's list of accounts and define a variable for the customer's specific account
            c.AddAccount(account);
            BankAccount customerAccount = c.accounts.First();

            // Defines the expected exception for this illegal operation (withdrawing a number equal to or less than zero)
            var exception = Assert.Throws<InvalidOperationException>(() => customerAccount.Withdraw(WITHDRAW_AMOUNT));
            Assert.Equal("Cannot withdraw nothing or a negative amount. ", exception.Message);


            // Ensures that the customer's account balance remains unchanged after an attempted "illegal" operation
            Assert.True(customerAccount.GetBalance() == initialBalance);
        }

        /// <summary>
        /// This test checks that the user is not able to withdraw a negativ amount of money from their account.
        /// </summary>
        [Fact]
        public void T_User_Cannot_Withdraw_Negative_Amount()
        {
            // Defining the "illegal" amount to be attempted withdrawn from the customer's account
            const int WITHDRAW_AMOUNT = -10;

            // Returns a new customer and account
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Defines initial balance, which is equal to the default balance found in the bank account's constructor
            int initialBalance = account.Balance;

            // Adds the returned account to the customer's list of accounts and define a variable for the customer's specific account
            c.AddAccount(account);
            BankAccount customerAccount = c.accounts.First();

            // Defines the expected exception to this illegal operation
            var exception = Assert.Throws<InvalidOperationException>(() => customerAccount.Withdraw(WITHDRAW_AMOUNT));
            Assert.Equal("Cannot withdraw nothing or a negative amount. ", exception.Message);

            // Ensures that the customer's account balance remains unchanged after an attempted "illegal" operation
            Assert.True(customerAccount.GetBalance() == initialBalance);
        }

        /// <summary>
        /// This feature test attempts to create a new customer with an invalid name (illegal characters) using the CustomerService.
        /// </summary>
        [Fact]
        public void T_Cannot_Validate_Customer_Name_With_Illegal_Characters()
        {
            // Initialise CustomerService
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);

            // Set const for name with illegal characters
            const string INVALID_NAME_ILLEGAL_CHARACTERS = "Hej/med*dig";

            // Retrieve output from name validation function
            bool customerNameValidated = service.ValidateCustomerName(INVALID_NAME_ILLEGAL_CHARACTERS);

            // Assert falsehood; this must return false, as the name contains illegal characters
            Assert.False(customerNameValidated);
        }

        /// <summary>
        /// This test attempts to create a new customer with an invalid name (more than two consecutive characters) using the CustomerService.
        /// </summary>
        [Fact]
        public void T_Cannot_Validate_Customer_Name_With_More_Than_Two_Consecutive_Characters()
        {
            // Initialise CustomerService
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);

            const string INVALID_NAME_CONSECUTIVE_CHARACTERS = "AAAAAAAAAAAAAAAAA";

            bool customerNameValidated = service.ValidateCustomerName(INVALID_NAME_CONSECUTIVE_CHARACTERS);

            Assert.False(customerNameValidated);
        }

        /// <summary>
        /// This test attempts to create a customer with an invalid e-mail address.
        /// </summary>
        [Fact]
        public void T_Cannot_Validate_Invalid_Customer_Email()
        {
            // Initialise CustomerService
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);

            // Set constant for invalid e-mail address
            const string INVALID_EMAIL = "fuck dig";

            // Retrieve output from e-mail validation function
            bool customerEmailValidated = service.ValidateCustomerEmail(INVALID_EMAIL);

            // Assert falsehood; this must always return false, as the input is not a valid e-mail address
            Assert.False(customerEmailValidated);
        }

        /// <summary>
        /// This test confirms that it is possible to create a customer with consecutive characters in their name. 
        /// The constraint is set that the customer's name must not contain more than two consecutive characters, as tested above. This is for safety. 
        /// </summary>
        [Fact]
        public void T_Can_Validate_Customer_Name_With_Consecutive_Characters()
        {
            // Initialise CustomerService
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);

            // Initialise name with consecutive characters
            const string NAME = "Waage Sandøe";

            // Retrieve output from name validation
            bool customerNameValidated = service.ValidateCustomerName(NAME);

            // Assert truth; the name is valid, and this function must always return true
            Assert.True(customerNameValidated);
        }

        /// <summary>
        /// This test confirms that it is possible to create a customer with dashes and apostrophes (- ') in their name.
        /// The constraint is set that the customer's name must not contain illegal characters, with an exemption set for dashes and apostrophes. 
        /// </summary>
        [Fact]
        public void T_Can_Validate_Customer_Name_With_Dashes_And_Apostrophes()
        {
            // Initialise CustomerService
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);

            // Set constant for valid name including dash and apostrophe
            const string NAME = "Jean-Claude D'Angelo";

            // Retrieve output from name validation function
            bool customerNameValidated = service.ValidateCustomerName(NAME);

            // Assert truth; this must always return true, as the value passed is a valid name
            Assert.True(customerNameValidated);
        }
    }
}