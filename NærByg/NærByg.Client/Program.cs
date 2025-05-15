using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using N�rByg.Client.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient
        {
            //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            BaseAddress = new Uri("https://nbapi.n�rbyg.dk/")
        });

        builder.Services.AddScoped<APIService>();

        await builder.Build().RunAsync();
    }
}