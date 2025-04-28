using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using N�rByg.Client.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped<APIService>();

        await builder.Build().RunAsync();
    }
}