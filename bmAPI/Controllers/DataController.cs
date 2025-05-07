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

        [HttpPost("brands")]
        public IActionResult ReturnBrands([FromBody] DataProviderRequest request)
        {
            List<BrandResponse> brands = _dataService.ReturnBrands(request.ChainId, request.LastSynced);

            return Ok(brands);
        }
        
        [HttpPost("categories")]
        public IActionResult ReturnCategories([FromBody] DataProviderRequest request)
        {
            List<CategoryResponse> categories = _dataService.ReturnCategories(request.ChainId, request.LastSynced);

            return Ok(categories);
        }

        [HttpPost("postareas")]
        public IActionResult ReturnPostAreas([FromBody] DataProviderRequest request)
        {
            List<PostAreaResponse> postAreas = _dataService.ReturnPostAreas(request.ChainId, request.LastSynced);

            return Ok(postAreas);
        }

        [HttpPost("products")]
        public IActionResult ReturnProducts([FromBody] DataProviderRequest request)
        {
            List<ProductResponse> products = _dataService.ReturnProducts(request.ChainId, request.LastSynced);

            return Ok(products);
        }

        [HttpPost("shops")]
        public IActionResult ReturnShops([FromBody] DataProviderRequest request)
        {
            List<ShopResponse> shops = _dataService.ReturnShops(request.ChainId, request.LastSynced);

            return Ok(shops);
        }

        [HttpPost("shopproducts")]
        public IActionResult ReturnShopProducts([FromBody] DataProviderRequest request)
        {
            List<MtmShopProductResponse> shopProducts = _dataService.ReturnShopProducts(request.ChainId, request.LastSynced);

            return Ok(shopProducts);
        }
    }
    
}
