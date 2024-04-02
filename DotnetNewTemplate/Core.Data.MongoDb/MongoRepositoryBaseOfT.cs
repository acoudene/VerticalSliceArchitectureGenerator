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

  public MongoRepositoryBase(IMongoContext mongoContext, string collectionName)
    : this(new MongoRepositoryComponent<TEntity, TMongoEntity>(mongoContext, collectionName))
  { }

  public MongoRepositoryBase(MongoRepositoryComponent<TEntity, TMongoEntity> component)
  {
    _mongoRepositoryComponent = component ?? throw new ArgumentNullException(nameof(component));
    _mongoRepositoryComponent.SetUniqueIndex(entity => entity.Id);
  }

  protected abstract TEntity ToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity ToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync() => await _mongoRepositoryComponent.GetAllAsync(ToEntity);

  public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _mongoRepositoryComponent.GetByIdAsync(id, ToEntity);

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids) => await _mongoRepositoryComponent.GetByIdsAsync(ids, ToEntity);

  public virtual async Task CreateAsync(TEntity newItem) => await _mongoRepositoryComponent.CreateAsync(newItem, ToMongoEntity);

  public virtual async Task UpdateAsync(TEntity updatedItem) => await _mongoRepositoryComponent.UpdateAsync(updatedItem, ToMongoEntity);

  public virtual async Task RemoveAsync(Guid id) => await _mongoRepositoryComponent.RemoveAsync(id);

  public virtual void SetUniqueIndex(params Expression<Func<TMongoEntity, object>>[] fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(IEnumerable<IndexKeysDefinition<TMongoEntity>> fields)
      => _mongoRepositoryComponent.SetUniqueIndex(fields);
}
