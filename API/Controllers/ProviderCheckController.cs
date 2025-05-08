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
        public async Task<IActionResult> GetProviderData([FromRoute] DataObjectType dataObject, [FromBody]ProviderRequest providerRequest, string username, string password)
        {
            // auth
            var authResponse = await _providerCheckService.LoginAsync(username, password);

            if (authResponse == null)            
                return BadRequest("Authentication failed");
            
            // Get data endpoints
            var data = await _providerDispatcherService.GetDataAsync(dataObject, providerRequest, authResponse.Token);

            if (data == null)
                return BadRequest($"Failed to fetch data for {dataObject}");

            return Ok(data);
       
        }
    }
}
