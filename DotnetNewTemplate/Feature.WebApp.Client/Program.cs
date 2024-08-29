using Feature.ViewModels;
using Feature.ViewModels.BffProxying;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IEntityNameViewModel, EntityNameViewModel>();
builder.Services
    .AddHttpClient(
    HttpEntityNameRestBffClient.ConfigurationName,
    client => client.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/EntityNameBff/"));
builder.Services.AddScoped<IEntityNameRestBffClient, HttpEntityNameRestBffClient>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
