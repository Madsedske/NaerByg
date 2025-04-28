using System.Net.Http.Json;

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
    }
}
