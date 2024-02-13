// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public abstract class MongoRepositoryBase<TEntity, TMongoEntity> : IRepository<TEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  protected MongoRepositoryComponent<TEntity, TMongoEntity> Component { get => _component; }
  private readonly MongoRepositoryComponent<TEntity, TMongoEntity> _component;

  public MongoRepositoryBase(IMongoContext mongoContext, string collectionName) 
    : this(new MongoRepositoryComponent<TEntity, TMongoEntity>(mongoContext, collectionName))
  { }

  public MongoRepositoryBase(MongoRepositoryComponent<TEntity, TMongoEntity> component) 
    => _component = component ?? throw new ArgumentNullException(nameof(component));

  protected abstract TEntity ToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity ToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync() => await _component.GetAllAsync(ToEntity);

  public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _component.GetByIdAsync(id, ToEntity);

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids) => await _component.GetByIdsAsync(ids, ToEntity);

  public virtual async Task CreateAsync(TEntity newItem) => await _component.CreateAsync(newItem, ToMongoEntity);

  public virtual async Task UpdateAsync(TEntity updatedItem) => await _component.UpdateAsync(updatedItem, ToMongoEntity);

  public virtual async Task RemoveAsync(Guid id) => await _component.RemoveAsync(id);

  public virtual void SetUniqueIndex(Expression<Func<TMongoEntity, object>> field) => _component.SetUniqueIndex(field);
}
