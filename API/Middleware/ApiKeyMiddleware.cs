using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NaerByg.Api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        private const string APIKEY_HEADER_NAME = "X-Api-Key";

        public ApiKeyMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {// Bypass API key check for Swagger UI and related endpoints
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (
                path.StartsWith("/swagger") ||
                path.StartsWith("/favicon") ||
                path.StartsWith("/index.html") || // optional if root redirects
                path.StartsWith("/_framework")    // Blazor support files
            ))
            {
                await _next(context);
                return;
            }


            if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
            {
                _logger.LogWarning("Request missing API key. Path: {Path}", context.Request.Path);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            var expectedApiKey = _configuration.GetValue<string>("Auth:APIKey");

            if (!expectedApiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Invalid API key used. Path: {Path}", context.Request.Path);
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            _logger.LogInformation("API key validated for path: {Path}", context.Request.Path);

            await _next(context);
        }
    }
}
