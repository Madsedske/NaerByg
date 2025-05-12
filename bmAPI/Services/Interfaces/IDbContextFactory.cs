using bmAPI.Services.Helpers;

namespace bmAPI.Services.Interfaces
{
    public interface IDbContextFactory
    {
        DatabaseContext Create(int chainId);
    }
}