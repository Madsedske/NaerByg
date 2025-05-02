using bmAPI.DTO;

namespace bmAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(AuthRequest authReq);
    }
}
