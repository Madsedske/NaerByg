using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.Xml;

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

        /// <summary>
        /// Retrieves a list of products that match the given search term.
        /// Returns a 404 response if no matching products are found.
        /// </summary>
        /// <param name="searchTerm">The term used to search for matching products.</param>
        /// <returns>
        /// An HTTP 200 OK response with the list of matching products, 
        /// or an HTTP 404 Not Found if no products are found.
        /// </returns>
        [HttpGet("GetProducts/{searchTerm}")]
        public IActionResult GetProducts(string searchTerm)
        {
            List<ProductsResponse> products = _productService.GetProducts(searchTerm);

            if (products == null || products.Count == 0)
                return NotFound($"No products found matching: {searchTerm}");

            return Ok(products);
        }
    }
}
