// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public abstract class MongoRepositoryBase<TEntity, TMongoEntity> : IRepository<TEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  protected MongoRepositoryComponent<TEntity, TMongoEntity> MongoRepositorycomponent { get => _mongoRepositorycomponent; }
  private readonly MongoRepositoryComponent<TEntity, TMongoEntity> _mongoRepositorycomponent;

  public MongoRepositoryBase(IMongoContext mongoContext, string collectionName)
    : this(new MongoRepositoryComponent<TEntity, TMongoEntity>(mongoContext, collectionName))
  { }

  public MongoRepositoryBase(MongoRepositoryComponent<TEntity, TMongoEntity> component)
  {
    _mongoRepositorycomponent = component ?? throw new ArgumentNullException(nameof(component));
    _mongoRepositorycomponent.SetUniqueIndex(entity => entity.Id);
  }

  protected abstract TEntity ToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity ToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync() => await _mongoRepositorycomponent.GetAllAsync(ToEntity);

  public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _mongoRepositorycomponent.GetByIdAsync(id, ToEntity);

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids) => await _mongoRepositorycomponent.GetByIdsAsync(ids, ToEntity);

  public virtual async Task CreateAsync(TEntity newItem) => await _mongoRepositorycomponent.CreateAsync(newItem, ToMongoEntity);

  public virtual async Task UpdateAsync(TEntity updatedItem) => await _mongoRepositorycomponent.UpdateAsync(updatedItem, ToMongoEntity);

  public virtual async Task RemoveAsync(Guid id) => await _mongoRepositorycomponent.RemoveAsync(id);

  public virtual void SetUniqueIndex(Expression<Func<TMongoEntity, object>> field) => _mongoRepositorycomponent.SetUniqueIndex(field);
}
