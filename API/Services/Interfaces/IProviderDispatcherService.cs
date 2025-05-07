using API.Enums;
using Shared.DTOs;

namespace API.Services.Interfaces
{
    public interface IProviderDispatcherService
    {
        Task<object?> GetDataAsync(DataObjectType dataObject, ProviderRequest request, string token);
    }
}
