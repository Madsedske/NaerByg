using Shared.DTOs;

namespace API.Services.Interfaces
{
    public interface IProviderDispatcherService
    {
        Task<object?> GetDataAsync(string dataObject, ProviderRequest request, string token);
    }
}
