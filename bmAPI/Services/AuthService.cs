using bmAPI.DTO;

namespace bmAPI.Services
{
    public class AuthService : IAuthService
    {        
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> Authenticate(AuthRequest authReq)
        {
            return "fe"; 
               //await _configuration.Get<AuthResponse>(authReq);
        }
    }
}
