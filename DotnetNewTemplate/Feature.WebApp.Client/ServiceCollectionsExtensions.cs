using Feature.ViewModels.BffProxying;
using Feature.ViewModels;

namespace Feature.WebApp.Client;

public static class ServiceCollectionsExtensions
{
  private static void AddClientsWithAddress(string baseAddress, Action<Uri> action)
  {
    if (string.IsNullOrWhiteSpace(baseAddress))
      throw new ArgumentNullException(nameof(baseAddress));

    if (action is null)
      throw new ArgumentNullException(nameof(action));

    var baseUri = new Uri(baseAddress);
    action(baseUri);
  }

  private static void AddClientsWithUri<TService, TImplementation>(this IServiceCollection serviceCollection, string name, Uri apiUri)
    where TService : class
    where TImplementation : class, TService
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentNullException(nameof(name));

    if (apiUri is null)
      throw new ArgumentNullException(nameof(apiUri));

    serviceCollection
      .AddHttpClient(name, client => client.BaseAddress = apiUri); // TODO - Adapt to manage ending slash on basse uri

    serviceCollection.AddScoped<TService, TImplementation>();
  }

  public static void AddViewModels(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IEntityNameViewModel, EntityNameViewModel>();
  }  

  public static void AddBffClients(this IServiceCollection serviceCollection, string baseAddress)
    => AddClientsWithAddress(baseAddress, (baseUri) => AddBffClients(serviceCollection, baseUri));

  public static void AddBffClients(this IServiceCollection serviceCollection, Uri baseUri)
    => serviceCollection
    .AddClientsWithUri<IEntityNameRestBffClient, HttpEntityNameRestBffClient>(
      HttpEntityNameRestBffClient.ConfigurationName,
      new Uri(baseUri, "api/EntityNameBff/"));
}
