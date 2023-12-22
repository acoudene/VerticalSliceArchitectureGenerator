namespace UseCase.Proxies;

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
