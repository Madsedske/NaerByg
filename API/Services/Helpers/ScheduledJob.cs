public abstract class ScheduledJob : BackgroundService
{
    private readonly TimeSpan _initialDelay;
    private readonly TimeSpan _interval;
    private readonly ILogger _logger;
    private readonly string _jobName;

    protected ScheduledJob(TimeSpan initialDelay, TimeSpan interval, ILogger logger, string jobName)
    {
        _initialDelay = initialDelay;
        _interval = interval;
        _logger = logger;
        _jobName = jobName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{_jobName} will start in {_initialDelay.TotalMinutes} minutes.");
        await Task.Delay(_initialDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation($"{_jobName} is running.");
                await RunJobAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{_jobName} failed.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    protected abstract Task RunJobAsync(CancellationToken stoppingToken);
}