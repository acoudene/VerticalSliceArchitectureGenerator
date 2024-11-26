// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels.BffProxying;

public class HttpEntityNameRestBffClientBehavior : HttpRestBffClientBehavior<EntityNameVo>
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClientBehavior(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }
}


