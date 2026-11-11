using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ContosoPizza.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUri = builder.Configuration["ApiBaseUrl"];
if (apiUri is null) throw new InvalidOperationException("API URL is not configured.");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUri) });

await builder.Build().RunAsync();
