using API.Enums;
using API.Services.Helpers;
using API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared.DTOs;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                throw new InvalidOperationException($"Configuration value for '{key}' is not a valid absolute URI: {value}");

            Console.WriteLine($"[CONFIG] {key} = {value}");

            return value;
        }

        private async Task<TResponse> FetchFromProvider<TResponse>(ProviderRequest request, string token, DataObjectType type)
        {
            var url = GetUrlFor(type);
            Console.WriteLine($"[API] Fetching from: {url}");

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Log request body
            Console.WriteLine($"[API] Sending request body: {JsonSerializer.Serialize(request)}");

            // Log headers
            Console.WriteLine("[API] Request headers:");
            foreach (var header in httpRequest.Headers)
            {
                Console.WriteLine($"[API]   {header.Key}: {string.Join(", ", header.Value)}");
            }

            if (httpRequest.Content?.Headers != null)
            {
                foreach (var header in httpRequest.Content.Headers)
                {
                    Console.WriteLine($"[API]   {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            var response = await _httpClient.SendAsync(httpRequest);
            Console.WriteLine($"[API] StatusCode: {response.StatusCode}");

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[API] Response body: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch data. Status: {response.StatusCode}, Body: {responseBody}");
            }

            try
            {
                var result = JsonSerializer.Deserialize<TResponse>(responseBody);
                return result ?? throw new JsonException("Deserialized response is null.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[API] Deserialization failed: {ex.Message}");
                throw;
            }
        }



        public async Task<AuthResponse> LoginAsync(string u, string p)
        {
            var requestUrl = "https://bmapi.xn--nrbyg-sra.dk/api/banana";

            var content = new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("u", u),
                 new KeyValuePair<string, string>("p", p)
            });

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<AuthResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return null;
            }


        }


        public Task<IEnumerable<ProviderProductResponse>> GetProductsData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<ProviderProductResponse>>(req, token, DataObjectType.Product);
        }

        public Task<IEnumerable<BrandResponse>> GetBrandsData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<BrandResponse>>(req, token, DataObjectType.Brand);
        }

        public Task<IEnumerable<CategoryResponse>> GetCategoriesData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<CategoryResponse>>(req, token, DataObjectType.Category);
        }

        public Task<IEnumerable<PostAreaResponse>> GetPostAreasData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<PostAreaResponse>>(req, token, DataObjectType.PostArea);
        }

        public Task<IEnumerable<ShopResponse>> GetShopsData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<ShopResponse>>(req, token, DataObjectType.Shop);
        }
        public Task<IEnumerable<MtmShopProductResponse>> GetMtmShopProductsData(ProviderRequest req, string token)
        {
            return FetchFromProvider<IEnumerable<MtmShopProductResponse>>(req, token, DataObjectType.MtmShopProduct);
        }
    }
}
