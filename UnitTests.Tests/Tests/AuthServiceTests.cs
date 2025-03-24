using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Services;
using UnitTests.Customers;
using UnitTests.Factories;

namespace UnitTests.Tests.Tests
{
    public class AuthServiceTests : IClassFixture<AuthServiceFixture>
    {
        private readonly AuthService _authService;
        private CustomerFactory _customerFactory;

        public AuthServiceTests(AuthServiceFixture fixture)
        {
            _authService = fixture.AuthService;
            _customerFactory = new CustomerFactory();
        }

        /// <summary>
        /// This test checks that it is possible to read an environment variable.
        /// </summary>
        [Fact]
        public void T_Can_Read_Environment_Variable()
        {
            // Retrieves signing key 'JWT_SECRET' from Environment Variables
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET", EnvironmentVariableTarget.User);
            Assert.False(string.IsNullOrWhiteSpace(jwtSecret), "JWT_SECRET is not set!");
        }

        /// <summary>
        /// This test checks that it is possible to generate a token based on the signing key.
        /// </summary>
        [Fact]
        public void T_Can_Generate_Token()
        {
            // Returns a fake customer from CustomerFactory
            Customer c = _customerFactory.Create();

            // Generates a token based on the ID of the returned customer
            var token = _authService.GenerateToken(c.Id.ToString());

            // Checks that the formatting matches the expected format of a JWT token and asserts that the token is not null or empty
            var parts = token.Split('.');
            Assert.Equal(3, parts.Length);
            Assert.False(string.IsNullOrWhiteSpace(token));
        }
    }

}
