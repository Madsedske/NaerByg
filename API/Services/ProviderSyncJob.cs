using API.Services.Helpers;

public class ProviderSyncJob : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProviderSyncJob> _logger;
    private readonly IConfiguration _configuration;
    private readonly ProviderSyncConfig _config;

    public ProviderSyncJob(
        IHttpClientFactory httpClientFactory,
        ILogger<ProviderSyncJob> logger,
        IConfiguration configuration,
        ProviderSyncConfig config)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting ProviderSyncJob for ChainId {ChainId} after {Delay} min.",
            _config.ChainId, _config.InitialDelay.TotalMinutes);

        await Task.Delay(_config.InitialDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var apiKey = _configuration["Auth:APIKey"];
                var chainId = _config.ChainId;

                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://xn--nrbyg-sra.dk/api/ProviderCheck/GetProviderData?chainId={chainId}");

                request.Headers.Add("x-api-key", apiKey);

                var response = await client.SendAsync(request, stoppingToken);
                var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Sync for ChainId {ChainId} succeeded: {Content}", chainId, responseContent);
                }
                else
                {
                    _logger.LogWarning("Sync for ChainId {ChainId} failed ({StatusCode}): {Content}",
                        chainId, response.StatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during ProviderSyncJob for ChainId {ChainId}", _config.ChainId);
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
