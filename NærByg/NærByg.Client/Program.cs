using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NærByg.Client.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient
        {
            //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            BaseAddress = new Uri("https://localhost:7045/")
        });

        builder.Services.AddScoped<APIService>();

        await builder.Build().RunAsync();
    }
}