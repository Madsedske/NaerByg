public class ProviderSyncJob1 : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProviderSyncJob1> _logger;
    private readonly IConfiguration _configuration;

    public ProviderSyncJob1(
        IHttpClientFactory httpClientFactory,
        ILogger<ProviderSyncJob1> logger,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProviderSyncJob1 started.");

        await Task.Delay(TimeSpan.FromMinutes(0), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var apiKey = _configuration["Auth:APIKey"];
                var chainId = 1;

                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://nbapi.xn--nrbyg-sra.dk/api/ProviderCheck/GetProviderData?chainId={chainId}");

                request.Headers.Add("x-api-key", apiKey);


                var response = await client.SendAsync(request, stoppingToken);
                var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("ProviderSyncJob1 succeeded: {Content}", responseContent);
                }
                else
                {
                    _logger.LogWarning("ProviderSyncJob1 failed with status {StatusCode}: {Content}", response.StatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProviderSyncJob1 encountered an error.");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
