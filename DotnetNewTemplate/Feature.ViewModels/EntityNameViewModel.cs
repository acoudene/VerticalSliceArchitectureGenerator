// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels;

/// <summary>
/// ViewModel associated to a dedicated entity
/// </summary>
public class EntityNameViewModel : IEntityNameViewModel
{
  private readonly EntityNameRestViewModelComponent _restViewModelComponent;
  public EntityNameViewModel(IEntityNameRestBffClient client)
  {
    _restViewModelComponent = new EntityNameRestViewModelComponent(client);
    Items = Enumerable.Empty<EntityNameVo>().ToList();
    SelectedItems = Enumerable.Empty<EntityNameVo>().ToHashSet();
  }

  public IEnumerable<EntityNameVo> Items { get; set; }
  public HashSet<EntityNameVo> SelectedItems { get; set; }
  public EntityNameVo? SelectedItem { get; set; }

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
