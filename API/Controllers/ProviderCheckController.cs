using API.Enums;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Shared.DTOs;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderCheckController : ControllerBase
    {
        private readonly IProviderCheckService _providerCheckService;

        public ProviderCheckController(IProviderCheckService ProviderCheckService)
        {
            
            _providerCheckService = ProviderCheckService;
        }


        [HttpPost("GetProviderData/{dataObject}")]
        public async Task<IActionResult> GetProviderData([FromRoute] DataObjectType dataObject, [FromBody] ProviderRequest request)
        {

            // auth
            var authResponse = new AuthResponse { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" };
                /*= await _providerCheckService.LoginAsync();
            if (authResponse == null)
                return BadRequest("Authentication failed");*/

            // calling method depending of DataObjectType given
            return dataObject switch
            {
                DataObjectType.Product => await TryGet(() => _providerCheckService.GetProductsData(request, authResponse.Token), "product"),
                DataObjectType.Brand => await TryGet(() => _providerCheckService.GetBrandsData(request, authResponse.Token), "brand"),
                DataObjectType.Shop => await TryGet(() => _providerCheckService.GetShopsData(request, authResponse.Token), "shop"),
                DataObjectType.Category => await TryGet(() => _providerCheckService.GetCategoriesData(request, authResponse.Token), "category"),
                DataObjectType.PostArea => await TryGet(() => _providerCheckService.GetPostAreasData(request, authResponse.Token), "post_area"),
                DataObjectType.MtmShopProduct => await TryGet(() => _providerCheckService.GetMtmShopProductsData(request, authResponse.Token), "mtm_shop_product"),
                _ => BadRequest("Unknown dataObject")
            };
        }

        private async Task<IActionResult> TryGet<T>(Func<Task<T>> fetchFunc, string label)
        {
            var result = await fetchFunc();
            return result == null ? BadRequest($"Failed to fetch {label} data") : Ok(result);
        }

       /* [HttpPost("GetProviderData/{dataObject}")] // chainname and datetime?
        public async Task<IActionResult> GetProviderData(string dataObject, [FromBody]ProviderRequest providerRequest)
        {


            // auth
            var authResponse = await _providerCheckService.LoginAsync();

            if (authResponse == null)
            {
                return BadRequest("Authentication failed");
            }

            // Check for which dataObject to get
            if (dataObject == "product")
            {
                var product = await _providerCheckService.GetProductsData(providerRequest, authResponse.Token);

                if (product == null)
                {
                    return BadRequest("Failed to fetch product data");
                }
                return Ok(product);
            }
            else if (dataObject == "brand")
            {
                var Brand = await _providerCheckService.GetBrandsData(providerRequest, authResponse.Token);

                if (Brand == null)
                {
                    return BadRequest("Failed to fetch Brand data");
                }
                return Ok(Brand);
            }
            else if (dataObject == "shop")
            {
                var Shop = await _providerCheckService.GetShopsData(providerRequest, authResponse.Token);

                if (Shop == null)
                {
                    return BadRequest("Failed to fetch Shop data");
                }
                return Ok(Shop);
            }
            else if (dataObject == "category")
            {
                var Categories = await _providerCheckService.GetCategoriesData(providerRequest, authResponse.Token);

                if (Categories == null)
                {
                    return BadRequest("Failed to fetch Categories data");
                }
                return Ok(Categories);
            }
            else if (dataObject == "post_area")
            {
                var PostArea = await _providerCheckService.GetPostAreasData(providerRequest, authResponse.Token);

                if (PostArea == null)
                {
                    return BadRequest("Failed to fetch PostArea data");
                }
                return Ok(PostArea);
            }
            else if (dataObject == "mtm_shop_product")
            {
                var MTMShopProduct = await _providerCheckService.GetMTMShopsProductsData(providerRequest, authResponse.Token);

                if (MTMShopProduct == null)
                {
                    return BadRequest("Failed to fetch MTMShopProduct data");
                }
                return Ok(MTMShopProduct);
            }
            else
            {
                return BadRequest("dataObject not found");
            }
        }*/
    }
}
