using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IGoogleService _googleService;

        public GoogleController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        [HttpGet("calculate")] 
        public async Task<ActionResult<GoogleDistanceResponse>> GetCalculatedDistance(string inputAddress, string shopAddress, string postarea)
        {
            var data = await _googleService.CalculateDistance(inputAddress, shopAddress, postarea);

            if (data == null) 
                return BadRequest($"Failed to fetch data for");

            return Ok(data);

        }
    }

}
