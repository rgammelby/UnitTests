using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Interfaces;

namespace UnitTests.Services
{
    public class ProfileService
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;

        public ProfileService(ICustomerService customerService, IAuthService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }

        // Function to simulate fetching the profile for a user using the JWT token
        public string GetProfile(string token)
        {
            // Validate the JWT token manually using the AuthService
            var principal = _authService.ValidateToken(token);

            if (principal == null)
            {
                return "Unauthorized: Invalid or expired token.";
            }

            // Extract the user ID from the ClaimsPrincipal
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))   
            {
                return "Unauthorized: Missing user ID in token.";
            }

            // Fetch user profile using the validated userId
            var userProfile = _customerService.GetCustomerProfile(userId);

            if (userProfile == null)
            {
                return "User profile not found.";
            }

            // Simulate returning the profile as a string
            return $"User Profile: {userProfile.Name}, {userProfile.Email}";
        }
    }
}
