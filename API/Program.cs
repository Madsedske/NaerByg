using API.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using API.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.Enums;
using Microsoft.OpenApi.Any;
using API.Services.Interfaces;
using NaerByg.Api.Middleware;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nærbyg API", Version = "v1" });

            c.UseInlineDefinitionsForEnums();
            c.MapType<DataObjectType>(() => new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetNames(typeof(DataObjectType))
                    .Select(name => new OpenApiString(name))
                    .Cast<IOpenApiAny>()
                    .ToList()
            });
        });

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("https://localhost:7252")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Dependency Injection
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProviderDispatcherService, ProviderDispatcherService>();
        builder.Services.AddHttpClient<IProviderCheckService, ProviderCheckService>();
        builder.Services.AddHttpClient<IGoogleService, GoogleService>();

        // Database Configuration
        var connectionString = builder.Configuration.GetConnectionString("connection");
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Authentication & Authorization
        var configuration = builder.Configuration;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(configuration.GetSection("AppSettings")["Token"] ?? throw new InvalidOperationException("Token is not configured in AppSettings"))),
                    ValidateAudience = false,
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("User", new AuthorizationPolicyBuilder()
                .RequireRole("User")
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });


        builder.Services.AddHttpClient(); // already used for services, safe to add again if not present

        // Register 3 scheduled jobs with different parameters
        builder.Services.AddSingleton<IHostedService>(sp =>
            new ProviderSyncJob(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ILogger<ProviderSyncJob>>(),
                sp.GetRequiredService<IConfiguration>(),
                new ProviderSyncConfig { ChainId = 1, InitialDelay = TimeSpan.FromMinutes(0) }
            ));

        builder.Services.AddSingleton<IHostedService>(sp =>
            new ProviderSyncJob(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ILogger<ProviderSyncJob>>(),
                sp.GetRequiredService<IConfiguration>(),
                new ProviderSyncConfig { ChainId = 2, InitialDelay = TimeSpan.FromMinutes(10) }
            ));

        builder.Services.AddSingleton<IHostedService>(sp =>
            new ProviderSyncJob(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ILogger<ProviderSyncJob>>(),
                sp.GetRequiredService<IConfiguration>(),
                new ProviderSyncConfig { ChainId = 3, InitialDelay = TimeSpan.FromMinutes(20) }
            ));

        var app = builder.Build();

        // HTTP and HTTPS Redirection
        app.UseHttpsRedirection();

        // Use CORS
        app.UseCors("AllowBlazorClient");

        // Use Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ApiKeyMiddleware>();
        // Swagger Setup
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nærbyg API v1");
                c.RoutePrefix = string.Empty; // Gør Swagger tilgængelig på roden
            });
        }

        app.MapControllers();
        app.Run();
    }
}
