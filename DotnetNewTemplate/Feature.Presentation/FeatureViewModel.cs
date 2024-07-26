namespace Feature.Presentation;

public class FeatureViewModel : IFeatureViewModel
{
  private readonly RestViewModelComponent<EntityNameVo, EntityNameDto, IEntityNameClient> _restViewModelComponent;
  public FeatureViewModel(RestViewModelComponent<EntityNameVo, EntityNameDto, IEntityNameClient> restViewModelComponent)
  {
    _restViewModelComponent = restViewModelComponent ?? throw new ArgumentNullException(nameof(restViewModelComponent));
  }

  protected virtual EntityNameVo ToViewObject(EntityNameDto dto)
    => dto.ToViewObject();

  protected virtual EntityNameDto ToDto(EntityNameVo viewObject)
    => viewObject.ToDto();

  public virtual async Task CreateAsync(EntityNameVo newItem, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.CreateAsync(newItem, ToDto, cancellationToken);

  public virtual async Task<List<EntityNameVo>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetAllAsync(ToViewObject, cancellationToken);

  public virtual async Task<EntityNameVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdAsync(id, ToViewObject, cancellationToken);

  public virtual async Task<List<EntityNameVo>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.GetByIdsAsync(ids, ToViewObject, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _restViewModelComponent.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, EntityNameVo updatedItem, CancellationToken cancellationToken = default)
   => await _restViewModelComponent.UpdateAsync(id, updatedItem, ToDto, cancellationToken);
}
