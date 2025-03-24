using UnitTests.Bank;
using UnitTests.Customers;
using UnitTests.Factories;
using UnitTests.Controllers;
using System.Reflection.Metadata;

namespace UnitTests.Tests.Tests
{
    public class DatabaseTests
    {
        // Initialise factories for customer and bank account creation
        private CustomerFactory _customerFactory;
        private BankAccountFactory _bankAccountFactory;

        public DatabaseTests()
        {
            _customerFactory = new CustomerFactory();
            _bankAccountFactory = new BankAccountFactory();
        }

        /// <summary>
        /// This test checks that it is possible to save a bank account to the temporary database and retrieve it.
        /// </summary>
        [Fact]
        public void T_Can_Save_And_Retrieve_Account()
        {
            // Initialise Database Context and return a new bank account
            using BankContext context = TestDbContextFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Sets expected bank account number for later operation
            int expectedAccountNumber = account.GetAccountNumber();

            // Adds the returned bank account to the temporary database's Accounts table
            context.Accounts.Add(account);
            context.SaveChanges();

            // Retrieves the bank account from the temporary database
            var retrieved = context.Accounts.First();

            // Checks that the bank account is not null, and matches its account number
            Assert.NotNull(retrieved);
            Assert.Equal(expectedAccountNumber, retrieved.AccountNumber);
        }

        /// <summary>
        /// This test has been abandoned, as EF Core's InMemory database apparently does not support uniqueness constraints.
        /// </summary>
        //[Fact]
        //public void T_Can_Save_Multiple_Accounts_With_Same_Account_Number_To_Database()
        //{
        //    using BankContext context = TestDbContextFactory.Create();
        //    BankAccount account1 = _bankAccountFactory.Create();
        //    BankAccount account2 = _bankAccountFactory.Create();
        //    BankAccount account3 = _bankAccountFactory.Create();

        //    context.Accounts.Add(account1);
        //    context.Accounts.Add(account2);
        //    context.Accounts.Add(account3);
        //    context.SaveChanges();
        //}

        /// <summary>
        /// This test checks that it is possible to save a user to the temporary database and retrieve it.
        /// </summary>
        [Fact]
        public void T_Can_Create_And_Retrieve_Customer()
        {
            // Initialises database context and returns a new customer
            using BankContext context = TestDbContextFactory.Create();
            Customer c = _customerFactory.Create();

            string expectedCustomerName = c.Name;

            // Saves the returned customer to the temporary database
            context.Customers.Add(c);
            context.SaveChanges();

            // Retrieves saved customer from the temporary database
            var retrieved = context.Customers.First(c => c.Name == expectedCustomerName);

            // Checks that the customer object is not null, and matches the expected customer name
            Assert.NotNull(retrieved);
            Assert.Equal(expectedCustomerName, retrieved.Name);
        }

        [Fact]
        public void T_Factory_Can_Generate_Valid_User_Properties()
        {
            using BankContext context = TestDbContextFactory.Create();
            Customer customer = _customerFactory.Create();

            Assert.NotNull(customer);
            Assert.NotNull(customer.Name);
            Assert.NotNull(customer.Email);
            Assert.NotNull(customer.Password);
        }

        /// <summary>
        /// This test checks whether it is possible to withdraw money from a bank account saved in the temporary database.
        /// </summary>
        [Fact]
        public void T_Can_Withdraw_From_And_Persist_Account_Balance()
        {
            // Initialise the amount to withdraw from the account
            const int WITHDRAW_AMOUNT = 5000;

            // Initialise database context and return new customer and bank account
            using BankContext context = TestDbContextFactory.Create();
            Customer c = _customerFactory.Create();
            BankAccount account = _bankAccountFactory.Create();

            // Initialise expected balance after withdrawal operation
            int expectedBalance = account.Balance - WITHDRAW_AMOUNT;

            // Bank account is added to the customer's list of bank accounts, and a new variable is initialised for the specific customer's account
            c.AddAccount(account);
            BankAccount customerAccount = c.accounts.First();

            // Customer and their bank account is written to the temporary database
            context.Customers.Add(c);
            context.Accounts.Add(customerAccount);
            context.SaveChanges();

            // Withdrawal operation is performed on the bank account
            customerAccount.Withdraw(WITHDRAW_AMOUNT);

            // Bank account is retrieved from the database, and its balance is matched to the expected balance
            var retrieved = context.Accounts.First();

            // Ensures that the balance of the retrieved bank account matches the expected balance
            Assert.Equal(expectedBalance, retrieved.Balance);
        }
    }
}
