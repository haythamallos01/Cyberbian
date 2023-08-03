using CyberbianSite.Client;
using CyberbianSite.Client.Config;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.SessionStorage;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using CyberbianSite.Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("CyberbianSite.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CyberbianSite.ServerAPI"));
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.Configure<ChatOptions>(o => builder.Configuration.GetSection("OpenAI").Bind(o));
builder.Services.Configure<DIDOptions>(o => builder.Configuration.GetSection("DID").Bind(o));
builder.Services.Configure<DatabaseOptions>(o => builder.Configuration.GetSection("Database").Bind(o));
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<DIDService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
