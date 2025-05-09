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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "N�rbyg API", Version = "v1" });

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


        var app = builder.Build();

        // HTTP and HTTPS Redirection
        app.UseHttpsRedirection();

        // Use CORS
        app.UseCors("AllowBlazorClient");

        // Use Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Swagger Setup
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "N�rbyg API v1");
                c.RoutePrefix = string.Empty; // G�r Swagger tilg�ngelig p� roden
            });
        }

        app.MapControllers();
        app.Run();
    }
}
