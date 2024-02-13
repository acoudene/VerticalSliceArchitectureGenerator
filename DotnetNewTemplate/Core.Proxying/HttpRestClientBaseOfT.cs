// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.Proxying;

public abstract class HttpRestClientBase<TDto> : IRestClient<TDto>
  where TDto : class, IIdentifierDto
{
  private readonly HttpRestClientComponent<TDto> _httpRestClientComponent;

  public abstract string GetConfigurationName();

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpRestClientBase(IHttpClientFactory httpClientFactory)
    : this(new HttpRestClientComponent<TDto>(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpRestClientBase(HttpRestClientComponent<TDto> httpRestClientComponent)
  {
    _httpRestClientComponent = httpRestClientComponent ?? throw new ArgumentNullException(nameof(httpRestClientComponent));
  }

  public virtual async Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);

  public virtual async Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      => await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);

  public virtual async Task<List<TDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
      => await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);

  public virtual async Task<HttpResponseMessage> CreateAsync(
    TDto dto,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
     => await _httpRestClientComponent.CreateAsync(dto, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);

  public virtual async Task<HttpResponseMessage> UpdateAsync(
    Guid id,
    TDto dto,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);

  public virtual async Task<TDto> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);

  public virtual async Task<HttpResponseMessage> PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patch,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
    => await _httpRestClientComponent.PatchAsync(id, patch, GetConfigurationName(), checkSuccessStatusCode, cancellationToken);
}


