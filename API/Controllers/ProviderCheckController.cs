using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Handles authenticated requests to retrieve external provider data based on the specified data object type.
        /// </summary>
        /// <param name="dataObject">The type of data to retrieve (e.g., 'product', 'brand', 'shop').</param>
        /// <param name="providerRequest">The request payload containing required parameters for the data fetch.</param>
        /// <returns>An IActionResult containing the retrieved data or an error response.</returns>
        [HttpPost("GetProviderData/{dataObject}")]
        public async Task<IActionResult> GetProviderData(string dataObject, [FromBody] ProviderRequest providerRequest)
        {
            // auth endpoint
            var authResponse = await _providerCheckService.LoginAsync();

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
