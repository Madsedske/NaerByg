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

        // Add services to the container
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
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Database Context via Factory
        builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
        builder.Services.AddScoped<IDataService, DataService>();

        // Authentication & Authorization
        var configuration = builder.Configuration;
        var tokenKey = builder.Configuration["AppSettings:Token"];
        var issuer = builder.Configuration["AppSettings:Issuer"];
        var audience = builder.Configuration["AppSettings:Audience"];

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true
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

       /* builder.Services.AddRateLimiter(options =>
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
        });*/

        var app = builder.Build();

       // app.UseRateLimiter();

        app.UseAuthentication();

        app.UseCors("AllowAll");

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
