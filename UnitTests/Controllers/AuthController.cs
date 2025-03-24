using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Services;

namespace UnitTests.Controllers
{
    public class AuthController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public string Login(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty.");
            }

            return _authService.GenerateToken(userId);
        }

        public ClaimsPrincipal Validate(string token)
        {
            return _authService.ValidateToken(token);
        }
    }

}
