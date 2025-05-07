using bmAPI.Services.Helpers;

namespace bmAPI.Services.Helpers
{
    public interface IDbContextFactory
    {
        DatabaseContext Create(int chainId);
    }
}
