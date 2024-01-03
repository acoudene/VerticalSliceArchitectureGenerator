// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Core.Proxying;

namespace Core.Presentation;

public abstract class RestViewModelBase<TViewObject, TDto> : IViewModel<TViewObject>
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
{
  private readonly IRestClient<TDto> _restClient;

  public RestViewModelBase(IRestClient<TDto> restClient)
  {
    _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
  }

  protected abstract TViewObject ToViewObject(TDto dto);
  protected abstract TDto ToDto(TViewObject viewObject);

  public virtual async Task CreateAsync(TViewObject newItem, bool checkSuccessStatusCode = true, CancellationToken cancellationToken = default)
  {
    await _restClient.CreateAsync(ToDto(newItem), checkSuccessStatusCode, cancellationToken);
  }

  public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return (await _restClient
      .GetAllAsync(cancellationToken))
      .Select(dto => ToViewObject(dto))
      .ToList();
  }

  public virtual async Task<TViewObject?> GetAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return ToViewObject((await _restClient
      .GetByIdAsync(id, cancellationToken)));
      
  }

  public virtual Task<List<TViewObject>?> GetAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
  {
    await _restClient.DeleteAsync(id, cancellationToken);
  }

  public virtual async Task UpdateAsync(Guid id, TViewObject updatedItem, bool checkSuccessStatusCode = true, CancellationToken cancellationToken = default)
  {
    await _restClient.UpdateAsync(id, ToDto(updatedItem), checkSuccessStatusCode, cancellationToken);
  }
}
