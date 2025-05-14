public class ProviderSyncJob2 : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProviderSyncJob2> _logger;
    private readonly IConfiguration _configuration;

    public ProviderSyncJob2(
        IHttpClientFactory httpClientFactory,
        ILogger<ProviderSyncJob2> logger,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProviderSyncJob2 will start in 10 minutes.");

        await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var apiKey = _configuration["Auth:APIKey"];
                var chainId = 2;

                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://nbapi.xn--nrbyg-sra.dk/api/ProviderCheck/GetProviderData?chainId={chainId}");

                request.Headers.Add("x-api-key", apiKey);

                var response = await client.SendAsync(request, stoppingToken);
                var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("ProviderSyncJob2 succeeded: {Content}", responseContent);
                }
                else
                {
                    _logger.LogWarning("ProviderSyncJob2 failed with status {StatusCode}: {Content}", response.StatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProviderSyncJob2 encountered an error.");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
