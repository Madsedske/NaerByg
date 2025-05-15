using Microsoft.AspNetCore.Mvc;
using bmAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using bmAPI.Services.Interfaces;

namespace bmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("brands")]
        public IActionResult ReturnBrands([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /categories called with chainId={request.ChainId}, lastSynced={request.LastSynced}");
            List<BrandResponse> brands = _dataService.ReturnBrands(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {brands.Count} brands");

            return Ok(brands);
        }
        
        [HttpPost("categories")]
        public IActionResult ReturnCategories([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /categories called with chainId={request.ChainId}, lastSynced={request.LastSynced}");

            List<CategoryResponse> categories = _dataService.ReturnCategories(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {categories.Count} categories");

            return Ok(categories);
        }

        [HttpPost("postareas")]
        public IActionResult ReturnPostAreas([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /postArea called with chainId={request.ChainId}, lastSynced={request.LastSynced}");
            List<PostAreaResponse> postAreas = _dataService.ReturnPostAreas(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {postAreas.Count} post areas");
            return Ok(postAreas);
        }

        [HttpPost("products")]
        public IActionResult ReturnProducts([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /products called with chainId={request.ChainId}, lastSynced={request.LastSynced}");
            List<ProductResponse> products = _dataService.ReturnProducts(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {products.Count} products");
            return Ok(products);
        }

        [HttpPost("shops")]
        public IActionResult ReturnShops([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /shops called with chainId={request.ChainId}, lastSynced={request.LastSynced}");
            List<ShopResponse> shops = _dataService.ReturnShops(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {shops.Count} shops");
            return Ok(shops);
        }

        [HttpPost("shopproducts")]
        public IActionResult ReturnShopProducts([FromBody] DataProviderRequest request)
        {
            Console.WriteLine($"[bmAPI] /shopProducts called with chainId={request.ChainId}, lastSynced={request.LastSynced}");
            List<MtmShopProductResponse> shopProducts = _dataService.ReturnShopProducts(request.ChainId, request.LastSynced);
            Console.WriteLine($"[bmAPI] Returning {shopProducts.Count} shop products");
            return Ok(shopProducts);
        }
    }
    
}
