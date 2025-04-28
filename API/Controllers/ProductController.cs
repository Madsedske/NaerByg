using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts/{searchTerm}")]
        public IActionResult GetProducts(string searchTerm)
        {
            var product = _productService.GetProducts(searchTerm);
            return Ok(product);
        }
    }
}
