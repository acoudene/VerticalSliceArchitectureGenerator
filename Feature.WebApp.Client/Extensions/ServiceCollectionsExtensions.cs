using Feature.ViewModels.BffProxying;
using Feature.ViewModels;

namespace Feature.WebApp.Client.Extensions;

public static class ServiceCollectionsExtensions
{
  public const string EntityNameBffApiRelativePath = "api/EntityNameBff/";

  public static void AddClientsWithUri<TService, TImplementation>(
    this IServiceCollection serviceCollection,
    string name,
    Uri apiUri)
    where TService : class
    where TImplementation : class, TService
  {

    // Think to adapt to manage ending slash on basse uri
    Func<IServiceCollection, string, Uri, IHttpClientBuilder> defaultHttpClientbuilder = (service, name, uri) 
      => service.AddHttpClient(name, client => client.BaseAddress = uri);
    
    serviceCollection
      .AddClientsWithUri<TService, TImplementation>(name, apiUri, defaultHttpClientbuilder);   
  }

public static void AddClientsWithUri<TService, TImplementation>(
    this IServiceCollection serviceCollection, 
    string name, 
    Uri apiUri, 
    Func<IServiceCollection, string, Uri, IHttpClientBuilder> httpClientbuilder)
    where TService : class
    where TImplementation : class, TService
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentNullException(nameof(name));

    if (apiUri is null)
      throw new ArgumentNullException(nameof(apiUri));

    if (httpClientbuilder is null)
      throw new ArgumentNullException(nameof(httpClientbuilder));

    httpClientbuilder(serviceCollection, name, apiUri);
    serviceCollection.AddScoped<TService, TImplementation>();
  }

  public static void AddViewModels(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IEntityNameViewModel, EntityNameViewModel>();
  }  

  public static void AddBffClients(this IServiceCollection serviceCollection, Uri baseUri)
    => serviceCollection
    .AddClientsWithUri<IEntityNameRestBffClient, HttpEntityNameRestBffClient>(
      HttpEntityNameRestBffClient.ConfigurationName,
      new Uri(baseUri, EntityNameBffApiRelativePath));
}
