using bmAPI.DTO;

namespace bmAPI.Services
{
    public interface IDataService
    {
        List<BrandResponse> ReturnBrands(int chainId, DateTime lastUpdated);
        List<CategoryResponse> ReturnCategories(int chainId, DateTime lastUpdated);
        List<MtmShopProductResponse> ReturnShopProducts(int chainId, DateTime lastUpdated);
        List<PostAreaResponse> ReturnPostAreas(int chainId, DateTime lastUpdated);
        List<ProductResponse> ReturnProducts(int chainId, DateTime lastUpdated);
        List<ShopResponse> ReturnShops(int chainId, DateTime lastUpdated);

    }
}
