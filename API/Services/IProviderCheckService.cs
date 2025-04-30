using Shared.DTOs;
using System.Net.Http;

namespace API.Services
{
    public interface IProviderCheckService
    {
        Task<AuthResponse> LoginAsync();
        Task<BrandsResponse> GetBrandsData(ProviderRequest providerReq, string token);
        Task<CategoriesResponse> GetCategoriesData(ProviderRequest providerReq, string token);
        Task<PostAreasResponse> GetPostAreasData(ProviderRequest providerReq, string token);
        Task<ProviderProductsResponse> GetProductsData(ProviderRequest providerReq, string token);
        Task<ShopsResponse> GetShopsData(ProviderRequest providerReq, string token);
        Task<MTMShopsProductsResponse> GetMTMShopsProductsData(ProviderRequest providerReq, string token);
    }
}
