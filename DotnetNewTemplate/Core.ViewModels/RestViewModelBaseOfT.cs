// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Core.ViewObjects;

namespace Core.ViewModels;

public abstract class RestViewModelBase<TViewObject, TRestBffClient> : IViewModel<TViewObject>
    where TViewObject : class, IIdentifierViewObject
    where TRestBffClient : IRestBffClient<TViewObject>
{
  private readonly RestViewModelComponent<TViewObject, TRestBffClient> _restViewModelComponent;

  public RestViewModelBase(RestViewModelComponent<TViewObject, TRestBffClient> restViewModelComponent)
      => _restViewModelComponent = restViewModelComponent ?? throw new ArgumentNullException(nameof(restViewModelComponent));

  public virtual async Task CreateAsync(TViewObject newItem, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.CreateAsync(newItem, cancellationToken);

  public virtual async Task CreateOrUpdateAsync(TViewObject newOrToUpdateVo, CancellationToken cancellationToken = default)
  => await _restViewModelComponent.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default)
      => await _restViewModelComponent.GetAllAsync(cancellationToken);

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<TViewObject>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, TViewObject updatedItem, CancellationToken cancellationToken = default)
   => await _restViewModelComponent.UpdateAsync(id, updatedItem, cancellationToken);

}
