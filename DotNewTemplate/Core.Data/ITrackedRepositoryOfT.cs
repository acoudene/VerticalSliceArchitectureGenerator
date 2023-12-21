namespace $safeprojectname$;

public interface ITrackedRepository<TEntity, TTrackedMetadata> : IRepository
  where TEntity : IEntity
  where TTrackedMetadata : ITrackedMetadata
{
  Task<List<TEntity>> GetAllAsync(TTrackedMetadata? trackedMetadata = default);

  Task<TEntity?> GetAsync(Guid id, TTrackedMetadata? trackedMetadata = default);

  Task<List<TEntity>?> GetAsync(List<Guid> ids, TTrackedMetadata? trackedMetadata = default);

  Task CreateAsync(TEntity newItem, TTrackedMetadata? trackedMetadata = default);

  Task UpdateAsync(TEntity updatedItem, TTrackedMetadata? trackedMetadata = default);

  Task RemoveAsync(Guid id, TTrackedMetadata? trackedMetadata = default);
}