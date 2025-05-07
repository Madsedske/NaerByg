using API.Enums;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<object?> GetDataAsync(DataObjectType dataObject, ProviderRequest request, string token)
        {
            return dataObject switch
            {
                DataObjectType.Product => await _providerCheckService.GetProductsData(request, token),
                DataObjectType.Brand => await _providerCheckService.GetBrandsData(request, token),
                DataObjectType.Shop => await _providerCheckService.GetShopsData(request, token),
                DataObjectType.Category => await _providerCheckService.GetCategoriesData(request, token),
                DataObjectType.PostArea => await _providerCheckService.GetPostAreasData(request, token),
                DataObjectType.MtmShopProduct => await _providerCheckService.GetMtmShopProductsData(request, token),
                _ => null
            };
        }
    }
}