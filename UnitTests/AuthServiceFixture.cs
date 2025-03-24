using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Services;

namespace UnitTests
{
    public class AuthServiceFixture
    {
        public AuthService AuthService { get; private set; }

        public AuthServiceFixture()
        {
            // Retrieve Environment Variable (generated externaly) or throw secret if not set 
            var signingKey = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new InvalidOperationException("JWT_SECRET is not set.");

            AuthService = new AuthService(signingKey);
        }
    }

}
