using bmAPI.DTO;
using bmAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bmAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public AuthResponse? Authenticate(string username, string password)
        {
            _logger.LogInformation("Authentication attempt for user: {Username}", username);

            var validUser = _configuration["AuthCredentials:Username"];
            var validPass = _configuration["AuthCredentials:Password"];

            if (username != validUser || password != validPass)
            {
                _logger.LogWarning("Authentication failed for user: {Username}", username);
                return null;
            };

            _logger.LogInformation("Authentication successful for user: {Username}", username);

            var issuer = "bmapi";
            var audience = "bmapi_clients";
            var key = _configuration["AppSettings:Token"]!;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiryTime = token.ValidTo
            };
        }
    }
}
