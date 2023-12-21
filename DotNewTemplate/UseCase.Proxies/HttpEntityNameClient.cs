namespace $safeprojectname$;

public class Http$ext_entityName$Client : HttpRestClientBase<$ext_entityName$Dto>, I$ext_entityName$Client
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public Http$ext_entityName$Client(IHttpClientFactory httpClientFactory)
    : base(httpClientFactory)
  {
  }

  public const string ConfigurationName = nameof(Http$ext_entityName$Client);

  public override string GetConfigurationName() => ConfigurationName;
}
