using API.Enums;
using API.Services.Helpers;
using API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared.DTOs;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

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

        private string GetUrlFor(DataObjectType type)
        {
            return type switch
            {
                DataObjectType.Product => GetUrlOrThrow("Endpoints:Product"),
                DataObjectType.Brand => GetUrlOrThrow("Endpoints:Brand"),
                DataObjectType.Category => GetUrlOrThrow("Endpoints:Category"),
                DataObjectType.Shop => GetUrlOrThrow("Endpoints:Shop"),
                DataObjectType.PostArea => GetUrlOrThrow("Endpoints:PostArea"),
                DataObjectType.MtmShopProduct => GetUrlOrThrow("Endpoints:MtmShopProduct"),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Unknown dataObject type")
            };
        }

        private string GetUrlOrThrow(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Missing or empty configuration value for '{key}'");

            return value;
        }

        private async Task<TResponse> FetchFromProvider<TResponse>(
            ProviderRequest request,
            string token,
            DataObjectType type
)
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
            var url = GetUrlFor(type);
            Console.WriteLine($"[API] Fetching from: {url}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(request)
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[API] StatusCode: {response.StatusCode}");
            Console.WriteLine($"[API] Raw body: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[API] Request failed.");
                return default;
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<TResponse>(responseBody);
                Console.WriteLine($"[API] Deserialization OK.");
                return parsed!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API] Deserialization failed: {ex.Message}");
                return default;
            }
        }



        /* private async Task<TResponse> FetchFromProvider<TResponse>(
             ProviderRequest request,
             string token,
             DataObjectType type
         )
         {
             var url = GetUrlFor(type);

             var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
             {
                 Content = JsonContent.Create(request)
             };
             httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

             var response = await _httpClient.SendAsync(httpRequest);
             if (!response.IsSuccessStatusCode) return default;

             return await response.Content.ReadFromJsonAsync<TResponse>();
         }

         */
        async Task<AuthResponse> IProviderCheckService.LoginAsync()
        {
            return await Task.FromResult(new AuthResponse
            {
                Token = "j09g3j75b954hg98h549hg895h4gh8954hgej"
            });
            /* var authRequest = new AuthRequest
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
             }*/
        }

        public Task<IEnumerable<ProviderProductResponse>> GetProductsData(ProviderRequest req, string token)
        {
            return FetchFromProvider< IEnumerable<ProviderProductResponse>>(req, token, DataObjectType.Product);
            
        }

        public Task<IEnumerable<BrandResponse>> GetBrandsData(ProviderRequest req, string token)
        {
            return FetchFromProvider< IEnumerable<BrandResponse>>(req, token, DataObjectType.Brand);

        }

        public Task<IEnumerable<CategoryResponse>> GetCategoriesData(ProviderRequest req, string token)
        {
            return FetchFromProvider< IEnumerable<CategoryResponse>>(req, token, DataObjectType.Category);

        }

        public Task<IEnumerable<PostAreaResponse>> GetPostAreasData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<PostAreaResponse>>(req, token, DataObjectType.PostArea);

        }

        public Task<IEnumerable<ShopResponse>> GetShopsData(ProviderRequest req, string token)
        {
            return FetchFromProvider< IEnumerable<ShopResponse>>(req, token, DataObjectType.Shop);

        }
        public Task<IEnumerable<MtmShopProductResponse>> GetMtmShopProductsData(ProviderRequest req, string token)
        {
            return FetchFromProvider< IEnumerable<MtmShopProductResponse>>(req, token, DataObjectType.MtmShopProduct);

        }
    }
}
