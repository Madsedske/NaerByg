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
        public async Task<AuthResponse> Authenticate(AuthRequest authReq)
        {
            return await _configuration.Get<AuthResponse>(authReq);
        }
    }
}
