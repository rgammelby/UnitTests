using UnitTests.Controllers;
using UnitTests.Customers;
using Bogus;
using System.Text.Json;
using UnitTests.Services;
using UnitTests.Interfaces;
using UnitTests.Repositories;

namespace UnitTests.Tests.Tests
{
    public class FeatureTests
    {
        public FeatureTests() { }

        // Initialise Faker for creating users with bogus data
        private static Faker<Customer> customerFaker = new Faker<Customer>()
            .RuleFor(customer => customer.Name, f => f.Name.FullName())
            .RuleFor(customer => customer.Email, f => f.Internet.Email())
            .RuleFor(customer => customer.Password, f => f.Internet.Password());

        // Initialise customer with Bogus data
        public static Customer CreateBogusCustomer()
        {
            return customerFaker.Generate();
        }

        /// <summary>
        /// This feature test attempts to create a new customer using the CustomerController and its associated 'Create' flow.
        /// </summary>
        [Fact]
        public void T_Can_Create_Customer_Using_Controller()
        {
            // Initialise CustomerController with all dependencies
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);
            CustomerController cc = new CustomerController(service);

            // Generate Bogus Customer
            Customer c = CreateBogusCustomer();

            // Serialise Bogus Customer into a JSON object
            string json = JsonSerializer.Serialize(c);

            // Pass Customer as JSON to controller's Create function
            Customer customer = cc.Create(json);

            // Checks that the returned customer is not null
            Assert.NotNull(customer);
        }

        /// <summary>
        /// This feature test checks that it is possible to create and retrieve a Customer with a JWT.
        /// </summary>
        [Fact]
        public void T_Can_Create_Customer_And_Retrieve_Customer_Information_Using_JWT()
        {
            // Initialise CustomerController with all dependencies
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);
            CustomerController cc = new CustomerController(service);

            // Initialise Authentication and Profile services
            AuthServiceFixture fixture = new AuthServiceFixture();
            // The AuthServiceFixture returns an AuthService instance initialised with a signing key from Environment Variables
            AuthService auth = fixture.AuthService;
            ProfileService profile = new ProfileService(service, auth);

            // Create Bogus Customer
            Customer c = CreateBogusCustomer();

            // Seralise Bogus Customer to JSON
            string json = JsonSerializer.Serialize(c);

            // Pass Customer JSON to Controller's Create function
            Customer customer = cc.Create(json);

            // Generates a token for the Customer based on ID
            var token = auth.GenerateToken(customer.Id.ToString());

            // Retrieves Customer Profile information based on their token
            var userInfo = profile.GetProfile(token);

            // Checks that good customer information is returned using their token
            Assert.NotNull(userInfo);
        }

        /// <summary>
        /// This test checks that it is not possible to retrieve a customer's profile information using an invalid token.
        /// </summary>
        [Fact]
        public void T_Cannot_Retrieve_Customer_Information_With_Invalid_Token()
        {
            // Initialise CustomerController with all dependencies
            CustomerRepository repo = new CustomerRepository();
            CustomerService service = new CustomerService(repo);
            CustomerController cc = new CustomerController(service);

            // Initialise Authentication and Profile services
            AuthServiceFixture fixture = new AuthServiceFixture();
            // The AuthServiceFixture returns an AuthService instance initialised with a signing key from Environment Variables
            AuthService auth = fixture.AuthService;
            ProfileService profile = new ProfileService(service, auth);

            // Create Bogus Customer
            Customer c = CreateBogusCustomer();

            // Seralise Bogus Customer to JSON
            string json = JsonSerializer.Serialize(c);

            // Pass Customer JSON to Controller's Create function
            Customer customer = cc.Create(json);

            // Initialise nonsense token instead of generating a proper one for the customer
            var token = "lmao idk what a token looks like";

            // Anticipates an UnauthorizedAccessException based on the invalid token
            var exception = Assert.Throws<UnauthorizedAccessException>(() => profile.GetProfile(token));
            Assert.Equal("Token validation failed. ", exception.Message);
        }
    }
}
