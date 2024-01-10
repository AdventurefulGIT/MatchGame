using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorMatchGame;
using BlazorMatchGame.Data;
using BlazorMatchGame.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<User>();
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddScoped<CredentialsService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

await builder.Build().RunAsync();

