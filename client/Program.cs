using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Application;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration.GetSection("ApiUri").Get<string>();
var apiKey = builder.Configuration.GetSection("ApiKey").Get<string>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl), DefaultRequestHeaders = { { "Key", apiKey } } });

builder.Services.AddMudServices();

await builder.Build().RunAsync();
