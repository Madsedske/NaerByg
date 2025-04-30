using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services.Helpers
{
    public class TokenManager
    {
        private ConfigurationManager _configuration;

        /// <summary>
        /// Method to create a token based on the username and admin_Yn.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="admin_Yn"></param>
        /// <returns> A token </returns>
        /// 
        public string CreateToken(string username, string admin_Yn)
        {
            _configuration = new ConfigurationManager();

            // sets the claims for token to use for authorize.
            List<Claim> claims = new List<Claim>
            {
            new Claim("User", username),
            new Claim("Role", "UserLoggedIn"),
            new Claim("Admin_Yn", admin_Yn)
            };

            // Gets the security key that a stored in appsettings.json
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
             _configuration.AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create the token with the claims, an expires date and the signingCredentials.
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
