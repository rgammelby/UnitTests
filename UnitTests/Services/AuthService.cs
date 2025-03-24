using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using UnitTests.Interfaces;

namespace UnitTests.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _signingKey;
        private readonly string _issuer = "Sascha";
        private readonly string _audience = "User";

        public AuthService(string signingKey)
        {
            _signingKey = signingKey;
        }

        public string GenerateToken(string userId)
        {
            var keyBytes = Convert.FromBase64String(_signingKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var keyBytes = Convert.FromBase64String(_signingKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = securityKey
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (SecurityTokenException)
            {
                throw new UnauthorizedAccessException("Token is invalid or expired. ");
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Token validation failed. ");
            }
        }

    }
}
