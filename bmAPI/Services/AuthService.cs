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

        public AuthResponse? Authenticate(string u, string p)
        {
            _logger.LogInformation("Authentication attempt for user: {U}", u);

            var validUser = Environment.GetEnvironmentVariable("var_username");
            var validPass = Environment.GetEnvironmentVariable("var_password");

            if (u != validUser || p != validPass)
            {
                _logger.LogWarning("Authentication failed for user: {U}", u);
                return null;
            };

            _logger.LogInformation("Authentication successful for user: {U}", u);

            var issuer = _configuration["AppSettings:Issuer"];
            var audience = _configuration["AppSettings:Audience"];
            var key = _configuration["AppSettings:Token"]!;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u),
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
