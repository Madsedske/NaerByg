using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
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

        /// <summary>
        /// Sends a request to the internal product API to retrieve a list of products matching the provided search term.
        /// </summary>
        /// <param name="productsRequest">The request containing the search term for product lookup.</param>
        /// <returns>A list of <see cref="ProductsResponse"/> objects matching the search criteria.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the API response is null, indicating a potential API issue.</exception>
        public async Task<List<ProductsResponse>> GetProductsFromSearched(ProductsRequest productsRequest)
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductsResponse>>($"api/Product/GetProducts/{productsRequest.SearchTerm}");

            return response ?? throw new InvalidOperationException("ProductsResponse cannot be null. API related issue.");
        }

        /// <summary>
        /// Calls an internal API endpoint to get the distance between the input address and the shop address 
        /// using Google Distance Matrix logic.
        /// </summary>
        /// <param name="inputAddress">The origin address entered by the user.</param>
        /// <param name="shopAddress">The destination shop address.</param>
        /// <param name="postarea">An additional location detail used in the distance calculation.</param>
        /// <returns>A <see cref="GoogleDistanceResponse"/> containing distance and duration details.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the response from the internal API is null.</exception>
        public async Task<GoogleDistanceResponse> GoogleMatrixAPI(string inputAddress, string shopAddress, string postarea)
        {
            var url = $"api/google/calculate?inputAddress={Uri.EscapeDataString(inputAddress)}&shopAddress={Uri.EscapeDataString(shopAddress)}&postarea={Uri.EscapeDataString(postarea)}";
        // Get all products from the searched
        public async Task<List<ProductResponse>> GetProductsFromSearched(ProductRequest productsRequest)
        {           
            var response = await _httpClient.GetFromJsonAsync<List<ProductResponse>>($"api/Product/GetProducts/{productsRequest.SearchTerm}");

            var result = await _httpClient.GetFromJsonAsync<GoogleDistanceResponse>(url);
            return result ?? throw new InvalidOperationException("Google response is null.");
        }
    }
}
