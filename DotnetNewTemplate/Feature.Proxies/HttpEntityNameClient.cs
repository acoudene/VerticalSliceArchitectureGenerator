// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Proxies;

public class HttpEntityNameClient : HttpRestClientBase<EntityNameDto>, IEntityNameClient
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameClient(IHttpClientFactory httpClientFactory)
    : base(httpClientFactory)
  {
  }

  public const string ConfigurationName = nameof(HttpEntityNameClient);

  public override string GetConfigurationName() => ConfigurationName;
}
