using Feature.Api.BackendForFrontend;
using Feature.WebApp.Client.Extensions;
using Feature.WebApp.Components;
using Feature.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddLocalization();

/// Add module to controller scanning, for clarty I have been redundant on controllers even if they share the same assembly 
builder.Services.AddControllersWithViews()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new AssemblyPart(typeof(EntityNameBffController).Assembly)))
                ;

// Add services to the container.
builder.Services.AddRazorComponents(options =>
    options.DetailedErrors = builder.Environment.IsDevelopment())
    .AddInteractiveWebAssemblyComponents();

const string bffApiBaseAddressesKey = "ASPNETCORE_URLS";
string bffApiBaseAddress = ((builder.Configuration[bffApiBaseAddressesKey] ?? string.Empty).Split(";").FirstOrDefault()) ?? string.Empty;

if (string.IsNullOrWhiteSpace(bffApiBaseAddress))
  throw new InvalidOperationException($"Missing value for configuration key: {bffApiBaseAddress}");

builder.Services.AddViewModels();
builder.Services.AddBffClients(new Uri(bffApiBaseAddress));

const string entityNameApiBaseAddressKey = "EntityName_API_BASEADDRESS";
string entityNameApiBaseAddress = builder.Configuration[entityNameApiBaseAddressKey] ?? string.Empty;
if (string.IsNullOrWhiteSpace(entityNameApiBaseAddress))
  throw new InvalidOperationException($"Missing value for configuration key: {entityNameApiBaseAddressKey}");

builder.Services.AddEntityNameApiClient(new Uri(entityNameApiBaseAddress));

builder.Services.AddMudServices();

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
