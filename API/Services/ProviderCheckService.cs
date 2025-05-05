using API.Services.Helpers;
using API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared.DTOs;
using System.Data;
using System.Net.Http.Headers;

namespace API.Services
{
    /// <summary>
    /// Handles communication with external provider APIs for authentication and data retrieval.
    /// Provides methods to authenticate using an API key and fetch various provider-related datasets such as products, brands, shops, and categories.
    /// </summary>
    /// <remarks>
    /// This service acts as a bridge between the system and external APIs, handling HTTP requests and response mapping.
    /// It uses an <see cref="HttpClient"/> instance and relies on configuration values for endpoint URLs and credentials.
    /// </remarks>
    public class ProviderCheckService : IProviderCheckService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProviderCheckService(DatabaseContext databaseContext, HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseContext = databaseContext;
            _httpClient = httpClient;
        }

        async Task<AuthResponse> IProviderCheckService.LoginAsync()
        {
            var authRequest = new AuthRequest
            {
                APIKey = _configuration["ProviderApi:Auth:APIKey"]
            };

            var authUrl = _configuration["ProviderApi:Auth:Url"];

            var response = await _httpClient.PostAsJsonAsync(authUrl, authRequest);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return authResponse;
            }
            else
            {
                return null;
            }
        }
        private async Task<T?> PostAuthorizedAsync<T>(string url, ProviderRequest req, string token)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(req)
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<T>()
                : default;
        }

        public Task<BrandsResponse> GetBrandsData(ProviderRequest req, string token) =>
             PostAuthorizedAsync<BrandsResponse>(url: _configuration["ProviderApi:BrandsUrl"], req, token);

        public Task<CategoriesResponse> GetCategoriesData(ProviderRequest req, string token) =>
            PostAuthorizedAsync<CategoriesResponse>(url: _configuration["ProviderApi:CategoriesUrl"], req, token);

        public Task<PostAreasResponse> GetPostAreasData(ProviderRequest req, string token) =>
            PostAuthorizedAsync<PostAreasResponse>(url: _configuration["ProviderApi:postAreaUrl"], req, token);

        public Task<ProviderProductsResponse> GetProductsData(ProviderRequest req, string token) =>
            PostAuthorizedAsync<ProviderProductsResponse>(url: _configuration["ProviderApi:ProductsUrl"], req, token);

        public Task<ShopsResponse> GetShopsData(ProviderRequest req, string token) =>
            PostAuthorizedAsync<ShopsResponse>(url: _configuration["ProviderApi:ShopsUrl"], req, token);

        public Task<MTMShopsProductsResponse> GetMTMShopsProductsData(ProviderRequest req, string token) =>
            PostAuthorizedAsync<MTMShopsProductsResponse>(url: _configuration["ProviderApi:mtm_shop_productUrl"], req, token);
    }
}
