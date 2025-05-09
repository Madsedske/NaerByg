using Shared.DTOs;
using System.Net.Http;

namespace API.Services.Interfaces
{
    public interface IProviderCheckService
    {
        Task<AuthResponse> LoginAsync(string username, string password);
        Task<IEnumerable<BrandResponse>> GetBrandsData(ProviderRequest providerReq, string token);
        Task<IEnumerable<CategoryResponse>> GetCategoriesData(ProviderRequest providerReq, string token);
        Task<IEnumerable<PostAreaResponse>> GetPostAreasData(ProviderRequest providerReq, string token);
        Task<IEnumerable<ProviderProductResponse>> GetProductsData(ProviderRequest providerReq, string token);
        Task<IEnumerable<ShopResponse>> GetShopsData(ProviderRequest providerReq, string token);
        Task<IEnumerable<MtmShopProductResponse>> GetMtmShopProductsData(ProviderRequest providerReq, string token);
    }
}
