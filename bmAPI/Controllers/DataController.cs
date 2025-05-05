using Microsoft.AspNetCore.Mvc;
using bmAPI.DTO;
using bmAPI.Services;

namespace bmAPI.Controllers
{
    [Route("bm/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("Brands/{chainId}")]
        public IActionResult ReturnBrands(int chainId, DateTime lastSynced)
        {
            List<BrandResponse> brands = _dataService.ReturnBrands(chainId, lastSynced);

            return Ok(brands);
        }
        
        [HttpGet("Categories/{chainId}")]
        public IActionResult ReturnCategories(int chainId, DateTime lastSynced)
        {
            List<CategoryResponse> categories = _dataService.ReturnCategories(chainId, lastSynced);

            return Ok(categories);
        }

        [HttpGet("PostAreas/{chainId}")]
        public IActionResult ReturnPostAreas(int chainId, DateTime lastSynced)
        {
            List<PostAreaResponse> postAreas = _dataService.ReturnPostAreas(chainId, lastSynced);

            return Ok(postAreas);
        }

        [HttpGet("Products/{chainId}")]
        public IActionResult ReturnProducts(int chainId, DateTime lastSynced)
        {
            List<ProductResponse> products = _dataService.ReturnProducts(chainId, lastSynced);

            return Ok(products);
        }

        [HttpGet("Shops/{chainId}")]
        public IActionResult ReturnShops(int chainId, DateTime lastSynced)
        {
            List<ShopResponse> shops = _dataService.ReturnShops(chainId, lastSynced);

            return Ok(shops);
        }

        [HttpGet("ShopProducts/{chainId}")]
        public IActionResult ReturnShopProducts(int chainId, DateTime lastSynced)
        {
            List<ShopProductResponse> shopProducts = _dataService.ReturnShopProducts(chainId, lastSynced);

            return Ok(shopProducts);
        }
    }
    
}
