// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.JsonPatch;

namespace Feature.Proxies;

public class HttpEntityNameClient : IEntityNameClient
{
  protected HttpRestClientComponent<EntityNameDto> HttpRestClientComponent { get => _httpRestClientComponent; }

  private readonly HttpRestClientComponent<EntityNameDto> _httpRestClientComponent;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameClient(IHttpClientFactory httpClientFactory)
  {
    _httpRestClientComponent = new HttpRestClientComponent<EntityNameDto>(httpClientFactory);
  }

  public const string ConfigurationName = nameof(HttpEntityNameClient);

  public virtual string GetConfigurationName() => ConfigurationName;

  public virtual async Task<List<EntityNameDto>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);

  public virtual async Task<EntityNameDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      => await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);

  public virtual async Task<List<EntityNameDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
      => await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);

  public virtual async Task<HttpResponseMessage> CreateAsync(
    EntityNameDto dto,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
     => await _httpRestClientComponent.CreateAsync(dto, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);

  public virtual async Task<HttpResponseMessage> UpdateAsync(
    Guid id,
    EntityNameDto dto,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);

  public virtual async Task<EntityNameDto> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);

  public virtual async Task<HttpResponseMessage> PatchAsync(
    Guid id,
    JsonPatchDocument<EntityNameDto> patch,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.PatchAsync(id, patch, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);
}
