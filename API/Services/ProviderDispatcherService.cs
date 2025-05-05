using API.Services.Interfaces;
using Shared.DTOs;

namespace API.Services
{
    public class ProviderDispatcherService : IProviderDispatcherService
    {
        private readonly IProviderCheckService _providerCheckService;

        public ProviderDispatcherService(IProviderCheckService providerCheckService)
        {
            _providerCheckService = providerCheckService;
        }

        public async Task<object?> GetDataAsync(string dataObject, ProviderRequest request, string token)
        {
            return dataObject switch
            {
                "product" => await _providerCheckService.GetProductsData(request, token),
                "brand" => await _providerCheckService.GetBrandsData(request, token),
                "shop" => await _providerCheckService.GetShopsData(request, token),
                "category" => await _providerCheckService.GetCategoriesData(request, token),
                "post_area" => await _providerCheckService.GetPostAreasData(request, token),
                "mtm_shop_product" => await _providerCheckService.GetMTMShopsProductsData(request, token),
                _ => null
            };
        }
    }

}
