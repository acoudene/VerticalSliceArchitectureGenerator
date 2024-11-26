// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels.Offline;

/// <summary>
/// ViewModel associated to a dedicated entity
/// </summary>
public class OfflineEntityNameViewModel : IEntityNameViewModel
{
  private readonly ICachingStorage _cachingStorage;
  private readonly EntityNameRestViewModelBehavior _behavior;

  public OfflineEntityNameViewModel(ICachingStorage cachingStorage, IEntityNameRestBffClient client)
  {
    _cachingStorage = cachingStorage ?? throw new ArgumentNullException(nameof(cachingStorage));

    ArgumentNullException.ThrowIfNull(client);

    _behavior = new EntityNameRestViewModelBehavior(client);
    Items = Enumerable.Empty<EntityNameVo>().ToList();
    SelectedItems = Enumerable.Empty<EntityNameVo>().ToHashSet();
  }

  public IEnumerable<EntityNameVo> Items { get; set; }
  public HashSet<EntityNameVo> SelectedItems { get; set; }
  public EntityNameVo? SelectedItem { get; set; }

  public virtual async Task CreateAsync(EntityNameVo newItem, CancellationToken cancellationToken = default)
    => await _behavior.CreateAsync(newItem, cancellationToken);

  public virtual async Task CreateOrUpdateAsync(EntityNameVo newOrToUpdateVo, CancellationToken cancellationToken = default)
    => await _behavior.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<EntityNameVo>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _behavior.GetAllAsync(cancellationToken);

  public virtual async Task<EntityNameVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<EntityNameVo>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, EntityNameVo updatedItem, CancellationToken cancellationToken = default)
   => await _behavior.UpdateAsync(id, updatedItem, cancellationToken);
}
