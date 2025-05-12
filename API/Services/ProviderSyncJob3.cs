public class ProviderSyncJob3 : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProviderSyncJob3> _logger;
    private readonly IConfiguration _configuration;

    public ProviderSyncJob3(
        IHttpClientFactory httpClientFactory,
        ILogger<ProviderSyncJob3> logger,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProviderSyncJob3 will start in 20 minutes.");

        await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var apiKey = _configuration["Auth:APIKey"];
                var chainId = 3;

                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://xn--nrbyg-sra.dk/api/ProviderCheck/GetProviderData?chainId={chainId}");

                request.Headers.Add("x-api-key", apiKey);

                var response = await client.SendAsync(request, stoppingToken);
                var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("ProviderSyncJob3 succeeded: {Content}", responseContent);
                }
                else
                {
                    _logger.LogWarning("ProviderSyncJob3 failed with status {StatusCode}: {Content}", response.StatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProviderSyncJob3 encountered an error.");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
