using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using Shared.DTOs;

namespace NærByg.Client.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // EKSEMPEL:
        /*
        // Fetch button configuration
        public async Task<ButtonConfigResponse> GetButtonConfig(string username)
        {
            var result = await _httpClient.GetFromJsonAsync<ButtonConfigResponse>($"api/Web/GetButtonConfig/{username}");

            return result ?? throw new InvalidOperationException("ButtonConfigResponse cannot be null. API related issue.");
        }*/

        // Get all products from the searched
        public async Task<List<ProductResponse>> GetProductsFromSearched(ProductRequest productsRequest)
        {           
            var response = await _httpClient.GetFromJsonAsync<List<ProductResponse>>($"api/Product/GetProducts/{productsRequest.SearchTerm}");

            return response ?? throw new InvalidOperationException("ProductsResponse cannot be null. API related issue.");
        }
    }
}
