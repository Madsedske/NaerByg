using bmAPI.DTO;

namespace bmAPI.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponse? Authenticate(string u, string p);
    }
}
