using API.Services.Interfaces;
using Shared.DTOs;
using System.Text.Json;

namespace API.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GoogleService(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GoogleDistanceResponse> CalculateDistance(string inputAddress, string shopAddress, string postarea)
        {

            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                     $"destinations={Uri.EscapeDataString(shopAddress + ", " + postarea)}&" +
                     $"origins={Uri.EscapeDataString(inputAddress)}&" +
                     $"units=metric&" +
                     $"key=AIzaSyBmuwpJh08co2X-MsuBZvUMNvh-zCp_t2E";

            var json = await _httpClient.GetStringAsync(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var responseData = JsonSerializer.Deserialize<GoogleDistanceResponse>(json, options);

            return responseData;
        }
    }
}
