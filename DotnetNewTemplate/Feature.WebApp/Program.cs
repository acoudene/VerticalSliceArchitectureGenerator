using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MudBlazor.Services;
using Feature.Api.BackendForFrontend;
using Feature.Proxies;
using Feature.ViewModels;
using Feature.ViewModels.BffProxying;
using Feature.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

/// Add module to controller scanning, for clarty I have been redundant on controllers even if they share the same assembly 
builder.Services.AddControllersWithViews()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new AssemblyPart(typeof(EntityNameBffController).Assembly)))
                ;

// Add services to the container.
builder.Services.AddRazorComponents(options =>
    options.DetailedErrors = builder.Environment.IsDevelopment())
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddScoped<IEntityNameViewModel, EntityNameViewModel>();

const string entityNameBffApiBaseAddressesKey = "ASPNETCORE_URLS";
string entityNameBffApiBaseAddress = ((builder.Configuration[entityNameBffApiBaseAddressesKey] ?? string.Empty).Split(";").FirstOrDefault()) ?? string.Empty;

if (string.IsNullOrWhiteSpace(entityNameBffApiBaseAddress))
  throw new InvalidOperationException($"Missing value for configuration key: {entityNameBffApiBaseAddress}");

builder.Services
    .AddHttpClient(
    HttpEntityNameRestBffClient.ConfigurationName,
    client => client.BaseAddress = new Uri(new Uri(entityNameBffApiBaseAddress), "api/EntityNameBff/"));
builder.Services.AddScoped<IEntityNameRestBffClient, HttpEntityNameRestBffClient>();

builder.Services.AddMudServices();

const string entityNameApiBaseAddressKey = "ENTITYNAME_API_BASEADDRESS";
string entityNameApiBaseAddress = builder.Configuration[entityNameApiBaseAddressKey] ?? string.Empty;
if (string.IsNullOrWhiteSpace(entityNameApiBaseAddress))
  throw new InvalidOperationException($"Missing value for configuration key: {entityNameApiBaseAddressKey}");

builder.Services
    .AddHttpClient(
    HttpEntityNameClient.ConfigurationName,
    client => client.BaseAddress = new Uri(entityNameApiBaseAddress));
builder.Services.AddScoped<IEntityNameClient, HttpEntityNameClient>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Feature.WebApp.Client._Imports).Assembly);

app.MapControllers();

app.Run();
