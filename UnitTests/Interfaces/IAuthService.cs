using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Services;

namespace UnitTests.Interfaces
{
    public interface IAuthService
    {
        public string GenerateToken(string userId);

        public ClaimsPrincipal ValidateToken(string token);
    }
}
