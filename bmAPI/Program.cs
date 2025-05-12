using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using bmAPI.Services.Helpers;
using bmAPI.Services;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using bmAPI.Services.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Logging (Console + File)
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add services to the container.
        builder.Services.AddControllers();

        // Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Byggemarked API", Version = "v1" });
            c.UseInlineDefinitionsForEnums();
        });

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("http://localhost:5003/")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
            });
        });

        // Database Context via Factory
        builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
        builder.Services.AddScoped<IDataService, DataService>();

        // Authentication & Authorization
        var configuration = builder.Configuration;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "bmapi",
                    ValidateAudience = true,
                    ValidAudience = "bmapi_clients",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration.GetSection("AppSettings")["Token"]
                        ?? throw new InvalidOperationException("Token is not configured in AppSettings")
                    )),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("User", new AuthorizationPolicyBuilder()
                .RequireRole("User")
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });

        builder.Services.AddRateLimiter(options =>
        {
            options.AddPolicy("AuthLimiter", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 50,
                        Window = TimeSpan.FromMinutes(60),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
        });

        var app = builder.Build();

        // HTTPS Redirection via IIS (Simply.com)
        app.UseHttpsRedirection();

        app.UseRateLimiter();

        app.UseAuthentication();

        app.UseCors("AllowBlazorClient");

        app.UseAuthorization();

        // Error Handling (Fejl logges i Console + logs/bmapi-log.txt)
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";

                var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                if (error != null)
                {
                    var logger = app.Services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(error.Error, "Der opstod en fejl: {Message}", error.Error.Message);

                    // Log til fil - opret mappe, hvis den ikke eksisterer
                    Directory.CreateDirectory("logs");
                    await File.AppendAllTextAsync("logs/bmapi-log.txt",
                        $"{DateTime.UtcNow} - ERROR: {error.Error.Message}\n{error.Error.StackTrace}\n\n");

                    await context.Response.WriteAsync("Der opstod en fejl. Tjek logs for mere information.");
                }
            });
        });

        // Swagger Setup
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Byggemarked API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.MapControllers();
        app.Run();
    }
}
