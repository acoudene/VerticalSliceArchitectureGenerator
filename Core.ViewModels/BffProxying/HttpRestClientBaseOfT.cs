// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Core.ViewModels.BffProxying;

public abstract class HttpRestBffClientBase<TViewObject> : IRestBffClient<TViewObject>
  where TViewObject : class, IIdentifierViewObject
{
    private readonly ILogger<HttpRestBffClientBase<TViewObject>> _logger;
    private readonly HttpRestBffClientComponent<TViewObject> _httpRestClientComponent;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public HttpRestBffClientBase(ILogger<HttpRestBffClientBase<TViewObject>> logger, IHttpClientFactory httpClientFactory)
      : this(logger, new HttpRestBffClientComponent<TViewObject>(httpClientFactory))
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public HttpRestBffClientBase(ILogger<HttpRestBffClientBase<TViewObject>> logger, HttpRestBffClientComponent<TViewObject> httpRestClientComponent)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpRestClientComponent = httpRestClientComponent ?? throw new ArgumentNullException(nameof(httpRestClientComponent));
    }

    public abstract string GetConfigurationName();

    public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
        return await _httpRestClientComponent.GetAllAsync(GetConfigurationName(), cancellationToken);
    }

    public virtual async Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
        return await _httpRestClientComponent.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
    }

    public virtual async Task<List<TViewObject>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
        return await _httpRestClientComponent.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
    }

    public virtual async Task CreateAsync(
        TViewObject dto,
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

    public virtual async Task CreateOrUpdateAsync(
        TViewObject dto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing call to {Method}({dto})...", nameof(CreateOrUpdateAsync), dto);

#if DEBUG // For security reasons      
        var response = await _httpRestClientComponent.CreateOrUpdateAsync(dto, GetConfigurationName(), false, cancellationToken);
        if (!response.IsSuccessStatusCode)
            _logger.LogDebug(response.Content.ReadAsStringAsync().Result);
        response.EnsureSuccessStatusCode();
#else
        await _httpRestClientComponent.CreateOrUpdateAsync(dto, GetConfigurationName(), true, cancellationToken);
#endif
    }

    public virtual async Task UpdateAsync(
      Guid id,
      TViewObject dto,
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

    public virtual async Task<TViewObject?> DeleteAsync(
      Guid id,
      CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing call to {Method}({id})...", nameof(DeleteAsync), id);
        return await _httpRestClientComponent.DeleteAsync(id, GetConfigurationName(), cancellationToken);
    }

    public virtual async Task PatchAsync(
      Guid id,
      JsonPatchDocument<TViewObject> patch,
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


