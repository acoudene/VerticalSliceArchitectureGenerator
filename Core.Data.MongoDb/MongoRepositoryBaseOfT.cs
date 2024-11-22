// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public abstract class MongoRepositoryBase<TEntity, TMongoEntity> : IRepository<TEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  protected MongoRepositoryComponent<TEntity, TMongoEntity> MongoRepositorycomponent { get => _mongoRepositoryComponent; }
  private readonly MongoRepositoryComponent<TEntity, TMongoEntity> _mongoRepositoryComponent;

  protected MongoRepositoryBase(IMongoContext mongoContext, string collectionName)
    : this(new MongoRepositoryComponent<TEntity, TMongoEntity>(mongoContext, collectionName))
  { }

  protected MongoRepositoryBase(MongoRepositoryComponent<TEntity, TMongoEntity> component)
  {
    _mongoRepositoryComponent = component ?? throw new ArgumentNullException(nameof(component));
    _mongoRepositoryComponent.SetUniqueIndex(entity => entity.Id);
  }

  protected abstract TEntity ToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity ToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.GetAllAsync(ToEntity, cancellationToken);

  public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.GetByIdAsync(id, ToEntity, cancellationToken);

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.GetByIdsAsync(ids, ToEntity, cancellationToken);

  public virtual async Task CreateAsync(TEntity newItem, CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.CreateAsync(newItem, ToMongoEntity, cancellationToken);

  public virtual async Task UpdateAsync(TEntity updatedItem, CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.UpdateAsync(updatedItem, ToMongoEntity, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _mongoRepositoryComponent.RemoveAsync(id, cancellationToken);

  public virtual void SetUniqueIndex(params Expression<Func<TMongoEntity, object>>[] fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params IndexKeysDefinition<TMongoEntity>[] fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);
}
