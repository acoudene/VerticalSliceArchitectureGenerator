// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels.BffProxying;

public class HttpEntityNameRestBffClientComponent : HttpRestBffClientComponent<EntityNameVo>
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClientComponent(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }
}


