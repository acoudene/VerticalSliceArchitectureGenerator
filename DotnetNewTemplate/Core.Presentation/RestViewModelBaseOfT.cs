// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;

namespace Core.Presentation;

public abstract class RestViewModelBase<TViewObject, TDto> : IViewModel<TViewObject>
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
{
  private readonly RestViewModelComponent<TViewObject, TDto> _restViewModelComponent;

  public RestViewModelBase(RestViewModelComponent<TViewObject, TDto> restViewModelComponent)
  {
    _restViewModelComponent = restViewModelComponent ?? throw new ArgumentNullException(nameof(restViewModelComponent));
  }

  protected abstract TViewObject ToViewObject(TDto dto);
  protected abstract TDto ToDto(TViewObject viewObject);

  public virtual async Task CreateAsync(TViewObject newItem, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.CreateAsync(newItem, ToDto, cancellationToken);

  public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetAllAsync(ToViewObject, cancellationToken);

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIDAsync(id, ToViewObject, cancellationToken);

  public virtual async Task<List<TViewObject>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdsAsync(ids, ToViewObject, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, TViewObject updatedItem, CancellationToken cancellationToken = default)
   => await _restViewModelComponent.UpdateAsync(id, updatedItem, ToDto, cancellationToken);

}
