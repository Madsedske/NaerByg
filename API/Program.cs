using API.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using API.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add CORS policy
       /* builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("http://192.168.1.36:5003/") // Update with your Blazor WASM app URL				 
                     .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });*/

        builder.Services.AddScoped<IProductService, ProductService>();
        /*builder.Services.AddScoped<IWebConfigService, WebConfigService>();
        builder.Services.AddScoped<IUserConfigService, UserConfigService>();*/

        // Added builder service and configuration for databasecontext with connectionstring to the startup for better dependency injection.
        var connectionString = builder.Configuration.GetConnectionString("connection");
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // Moved connectionstring to program.cs from DbContext

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run("http://192.168.1.36:5001/");
        //app.Run();
    }
}