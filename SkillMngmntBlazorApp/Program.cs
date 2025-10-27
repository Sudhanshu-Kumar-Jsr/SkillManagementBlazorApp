using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SkillMngmntBlazorApp;
using SkillMngmntBlazorApp.Services;
using SkillMngmntBlazorApp.Services.SkillMngmntBlazorApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register Blazored Local Storage
builder.Services.AddBlazoredLocalStorage();

// Register JwtAuthStateProvider for AuthenticationStateProvider
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthStateProvider>());

// Add authorization support
builder.Services.AddAuthorizationCore();

// Register AuthMessageHandler and HttpClient using IHttpClientFactory
builder.Services.AddTransient<AuthMessageHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5132/");
})
.AddHttpMessageHandler<AuthMessageHandler>();

// Provide HttpClient for injection
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<AdminService>();

await builder.Build().RunAsync();
