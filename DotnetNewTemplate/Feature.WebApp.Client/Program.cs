using Feature.WebApp.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddViewModels();
builder.Services.AddBffClients(builder.HostEnvironment.BaseAddress);

builder.Services.AddMudServices();

await builder.Build().RunAsync();
