using CyberbianSite.Client;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("CyberbianSite.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CyberbianSite.ServerAPI"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.Configure<ChatOptions>(o => builder.Configuration.GetSection("OpenAI").Bind(o));
builder.Services.AddScoped<OpenAIService>();


await builder.Build().RunAsync();
