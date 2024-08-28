// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Feature.ViewObjects;

namespace Feature.ViewModels.BffProxying;

public class HttpEntityNameRestBffClient : IEntityNameRestBffClient
{
  private readonly ILogger<HttpEntityNameRestBffClient> _logger;
  private readonly HttpEntityNameRestBffClientComponent _httpRestClientComponent;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClient(ILogger<HttpEntityNameRestBffClient> logger, IHttpClientFactory httpClientFactory)
    : this(logger, new HttpEntityNameRestBffClientComponent(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClient(ILogger<HttpEntityNameRestBffClient> logger, HttpEntityNameRestBffClientComponent httpRestClientComponent)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _httpRestClientComponent = httpRestClientComponent ?? throw new ArgumentNullException(nameof(httpRestClientComponent));
  }

  public const string ConfigurationName = nameof(HttpEntityNameRestBffClient);
  public virtual string GetConfigurationName() => ConfigurationName;

  public virtual async Task<List<EntityNameVo>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
    return await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<EntityNameVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
    return await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<EntityNameVo>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
    return await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
      EntityNameVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({vo})...", nameof(CreateAsync), vo);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.CreateAsync(vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.CreateAsync(dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task CreateOrUpdateAsync(
      EntityNameVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({vo})...", nameof(CreateOrUpdateAsync), vo);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.CreateOrUpdateAsync(vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.CreateOrUpdateAsync(dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task UpdateAsync(
    Guid id,
    EntityNameVo vo,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({id},{vo})...", nameof(UpdateAsync), id, vo);

#if DEBUG // For security reasons      
    var response = await _httpRestClientComponent.UpdateAsync(id, vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.UpdateAsync(id, dto, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task<EntityNameVo?> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({id})...", nameof(DeleteAsync), id);
    return await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<EntityNameVo> patch,
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


