using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Rentbook;
using Rentbook.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Toast;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");




// Load configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddRadzenComponents();
// Add HttpClient for local file access
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredToast();
// Configure the named HttpClient for your API
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("API", (sp, client) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiBaseUrl = configuration["ApiBaseUrl"];
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new InvalidOperationException("API Base URL is not configured.");
    }
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();

var houseApiBaseUrl = builder.Configuration["ApiBaseUrl"];
builder.Services.AddHttpClient<IHouseService, HouseService>(client =>
{
    if (string.IsNullOrEmpty(houseApiBaseUrl))
    {
        throw new InvalidOperationException("House API Base URL is not configured.");
    }
    client.BaseAddress = new Uri(houseApiBaseUrl);
}).AddHttpMessageHandler<AuthorizationMessageHandler>();
//builder.Services.AddHttpClient<IHouseService,HouseService>();
builder.Services.AddScoped<HouseService>();

// Add Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Configure authentication
builder.Services.AddAuthorizationCore();




var app = builder.Build();


await builder.Build().RunAsync();