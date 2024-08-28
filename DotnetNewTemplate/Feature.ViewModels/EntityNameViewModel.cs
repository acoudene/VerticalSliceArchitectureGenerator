using Core.ViewObjects;
using Feature.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels;

public class EntityNameViewModel : IEntityNameViewModel
{
  private readonly EntityNameRestViewModelComponent _restViewModelComponent;
  public EntityNameViewModel(IEntityNameRestBffClient client)
  {
    _restViewModelComponent = new EntityNameRestViewModelComponent(client);
  }

  public virtual async Task CreateAsync(EntityNameVo newItem, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.CreateAsync(newItem, cancellationToken);

  public virtual async Task CreateOrUpdateAsync(EntityNameVo newOrToUpdateVo, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<EntityNameVo>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetAllAsync(cancellationToken);

  public virtual async Task<EntityNameVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<EntityNameVo>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, EntityNameVo updatedItem, CancellationToken cancellationToken = default)
   => await _restViewModelComponent.UpdateAsync(id, updatedItem, cancellationToken);
}
