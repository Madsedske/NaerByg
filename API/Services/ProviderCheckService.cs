using API.Services.Helpers;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared.DTOs;
using Shared.Models;
using System.Data;
using System.Net.Http.Headers;

namespace API.Services
{
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
                APIKey = _configuration["Auth:APIKey"]
            };

            var authUrl = _configuration["Auth:Url"];

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

        public async Task<BrandsResponse> GetBrandsData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<BrandsResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }

        public async Task<CategoriesResponse> GetCategoriesData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<CategoriesResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }

        public async Task<PostAreasResponse> GetPostAreasData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<PostAreasResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }

        public async Task<ProviderProductsResponse> GetProductsData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<ProviderProductsResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }

        public async Task<ShopsResponse> GetShopsData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<ShopsResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }

        public async Task<MTMShopsProductsResponse> GetMTMShopsProductsData(ProviderRequest providerReq, string token)
        {
            var productUrl = _configuration["Product:Url"];

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, productUrl)
            {
                Content = JsonContent.Create(providerReq)
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                var productResponse = await response.Content.ReadFromJsonAsync<MTMShopsProductsResponse>();
                return productResponse;
            }
            else
            {
                return null; // or throw new Exception("Product fetch failed");
            }
        }
    }
}
