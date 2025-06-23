using AgendaApp.BlazorWasm.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AgendaApp.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // HTTP Client
            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri("https://localhost:7000/") // URL da Web API
            });

            // Local Storage
            builder.Services.AddBlazoredLocalStorage();

            // Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
