﻿using API.Services.Interfaces;
using Shared.DTOs;
using System.Text.Json;

namespace API.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly HttpClient _httpClient;

        public GoogleService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleDistanceResponse> CalculateDistance(string inputAddress, string shopAddress, int shopPostArea)
        {

            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                     $"destinations={Uri.EscapeDataString(shopAddress + ", " + shopPostArea)}&" +
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
