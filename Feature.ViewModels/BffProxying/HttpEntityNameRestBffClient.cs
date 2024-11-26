// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Feature.ViewModels.BffProxying;

public class HttpEntityNameRestBffClient : IEntityNameRestBffClient
{
  private readonly ILogger<HttpEntityNameRestBffClient> _logger;
  private readonly HttpEntityNameRestBffClientBehavior _behavior;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClient(ILogger<HttpEntityNameRestBffClient> logger, IHttpClientFactory httpClientFactory)
    : this(logger, new HttpEntityNameRestBffClientBehavior(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpEntityNameRestBffClient(ILogger<HttpEntityNameRestBffClient> logger, HttpEntityNameRestBffClientBehavior behavior)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
  }

  public const string ConfigurationName = nameof(HttpEntityNameRestBffClient);
  public virtual string GetConfigurationName() => ConfigurationName;

  public virtual async Task<List<EntityNameVo>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
    return await _behavior.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<EntityNameVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
    return await _behavior.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<EntityNameVo>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
    return await _behavior.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
      EntityNameVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Vo})...", nameof(CreateAsync), vo);

#if DEBUG // For security reasons      
    var response = await _behavior.CreateAsync(vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
    await _httpRestClientComponent.CreateAsync(vo, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task CreateOrUpdateAsync(
      EntityNameVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Vo})...", nameof(CreateOrUpdateAsync), vo);

#if DEBUG // For security reasons      
    var response = await _behavior.CreateOrUpdateAsync(vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
    await _httpRestClientComponent.CreateOrUpdateAsync(vo, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task UpdateAsync(
    Guid id,
    EntityNameVo vo,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Vo})...", nameof(UpdateAsync), id, vo);

#if DEBUG // For security reasons      
    var response = await _behavior.UpdateAsync(id, vo, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
    await _httpRestClientComponent.UpdateAsync(id, vo, GetConfigurationName(), true, cancellationToken);
#endif
  }

  public virtual async Task<EntityNameVo?> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(DeleteAsync), id);
    return await _behavior.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<EntityNameVo> patch,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Patch})...", nameof(PatchAsync), id, patch);

#if DEBUG // For security reasons      
    var response = await _behavior.PatchAsync(id, patch, GetConfigurationName(), false, cancellationToken);
    if (!response.IsSuccessStatusCode)
      _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
#else
    await _httpRestClientComponent.PatchAsync(id, patch, GetConfigurationName(), true, cancellationToken);
#endif
  }
}


