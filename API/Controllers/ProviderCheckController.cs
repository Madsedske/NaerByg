using API.Enums;
using API.Services;
using API.Services.Interfaces;
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
        private readonly IProviderDispatcherService _providerDispatcherService;

        public ProviderCheckController(IProviderCheckService ProviderCheckService, IProviderDispatcherService ProviderDispatcherService)
        {
            _providerDispatcherService = ProviderDispatcherService;
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
                return BadRequest("Authentication failed");
            
            // Get data endpoints
            var data = await _providerDispatcherService.GetDataAsync(dataObject, providerRequest, authResponse.Token);

            if (data == null)
                return BadRequest($"Failed to fetch data for {dataObject}");

            return Ok(data);
       
        }*/
    }
}
