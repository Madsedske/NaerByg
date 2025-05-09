using bmAPI.DTO;

namespace bmAPI.Services
{
    public interface IAuthService
    {
        AuthResponse? Authenticate(string username, string password);
    }
}
