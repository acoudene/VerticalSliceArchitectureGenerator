// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Core.Proxying;

public abstract class HttpRestClientBase<TDto> : IRestClient<TDto>
  where TDto : class, IIdentifierDto
{
  private readonly ILogger<HttpRestClientBase<TDto>> _logger;
  private readonly HttpRestClientComponent<TDto> _httpRestClientComponent;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpRestClientBase(ILogger<HttpRestClientBase<TDto>> logger, IHttpClientFactory httpClientFactory)
    : this(logger, new HttpRestClientComponent<TDto>(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpRestClientBase(ILogger<HttpRestClientBase<TDto>> logger, HttpRestClientComponent<TDto> httpRestClientComponent)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _httpRestClientComponent = httpRestClientComponent ?? throw new ArgumentNullException(nameof(httpRestClientComponent));
  }

  public abstract string GetConfigurationName();

  public virtual async Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
    return await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
    return await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<TDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
    return await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
    TDto dto,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({dto})...", nameof(CreateAsync), dto);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.CreateAsync(dto, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.CreateAsync(dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task UpdateAsync(
    Guid id,
    TDto dto,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({id},{dto})...", nameof(UpdateAsync), id, dto);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task<TDto> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({id})...", nameof(DeleteAsync), id);
    return await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patch,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({id},{patch})...", nameof(PatchAsync), id, patch);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.PatchAsync(id, patch, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.PatchAsync(id, patch, GetConfigurationName(), true, cancellationToken);
#endif
  }
}


