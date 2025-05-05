using bmAPI.DTO;

namespace bmAPI.Services
{
    public interface IAuthService
    {
        Task<string> Authenticate(AuthRequest authReq);
    }
}
