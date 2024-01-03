// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Mvc.Testing;

namespace Core.Host.Testing;

public class TestHttpClientFactory<TEntryPoint> : IHttpClientFactory where TEntryPoint : class
{
  private readonly WebApplicationFactory<TEntryPoint> _appFactory;
  private readonly WebApplicationFactoryClientOptions _options;

  public TestHttpClientFactory(WebApplicationFactory<TEntryPoint> appFactory, WebApplicationFactoryClientOptions options)
  {
    if (appFactory is null)
      throw new ArgumentNullException(nameof(appFactory));
    if (options is null)
      throw new ArgumentNullException(nameof(options));

    _appFactory = appFactory;
    _options = options;
  }

  public virtual HttpClient CreateClient(string name) => _appFactory.CreateClient(_options);


  private static Uri GenerateUrl(Uri baseAddress, string relativePath)
    => new Uri($"{baseAddress.AbsoluteUri.TrimEnd('/')}{relativePath}");

  private static WebApplicationFactoryClientOptions SetBaseAddress(WebApplicationFactoryClientOptions options, string relativePath)
  {
    options.BaseAddress = GenerateUrl(options.BaseAddress, relativePath);
    return options;
  }

  public static IHttpClientFactory CreateHttpClientFactory(
    WebApplicationFactory<TEntryPoint> webApplicationFactory,
    string relativePath,
    WebApplicationFactoryClientOptions? givenOptions = null)
  {
    if (givenOptions is null)
      givenOptions = new WebApplicationFactoryClientOptions();

    var options = SetBaseAddress(givenOptions, relativePath);
    return new TestHttpClientFactory<TEntryPoint>(webApplicationFactory, options);
  }
}