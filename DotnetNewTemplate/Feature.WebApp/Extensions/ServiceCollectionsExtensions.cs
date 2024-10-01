using Feature.Proxies;
using Feature.WebApp.Client.Extensions;

namespace Feature.WebApp.Extensions;

public static class ServiceCollectionsExtensions
{
  public static void AddEntityNameApiClient(this IServiceCollection serviceCollection, Uri apiUri)
    => serviceCollection
    .AddClientsWithUri<IEntityNameClient, HttpEntityNameClient>(
      HttpEntityNameClient.ConfigurationName,
      apiUri);
}
