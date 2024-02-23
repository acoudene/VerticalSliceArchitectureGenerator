// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Feature.Proxies;

public class HttpEntityNameClient : IEntityNameClient
{
  private readonly ILogger<HttpEntityNameClient> _logger;

  protected HttpRestClientComponent<EntityNameDto> HttpRestClientComponent { get => _httpRestClientComponent; }
  private readonly HttpRestClientComponent<EntityNameDto> _httpRestClientComponent;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="logger"></param>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameClient(ILogger<HttpEntityNameClient> logger, IHttpClientFactory httpClientFactory)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _httpRestClientComponent = new HttpRestClientComponent<EntityNameDto>(httpClientFactory);
  }

  public const string ConfigurationName = nameof(HttpEntityNameClient);

  public virtual string GetConfigurationName() => ConfigurationName;

  public virtual async Task<List<EntityNameDto>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}...", nameof(GetAllAsync));

    return await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<EntityNameDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Id})...", nameof(GetByIdAsync), id);

    return await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<EntityNameDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));

    return await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
    EntityNameDto dto,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Dto})...", nameof(CreateAsync), dto);

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
    EntityNameDto dto,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Id},{Dto})...", nameof(UpdateAsync), id, dto);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task<EntityNameDto?> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Id})...", nameof(DeleteAsync), id);

    return await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<EntityNameDto> patch,    
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing remote call to {Method}({Id},{Patch})...", nameof(PatchAsync), id, patch);

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
