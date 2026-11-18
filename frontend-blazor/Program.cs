using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ContosoPizza.Client;
using ContosoPizza.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUri = builder.Configuration["ApiBaseUrl"];
if (apiUri is null) throw new InvalidOperationException("API URL is not configured.");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUri) });

builder.Services.AddScoped<ITokenStorage, TokenStorage>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthenticationStateProvider>());

await builder.Build().RunAsync();
