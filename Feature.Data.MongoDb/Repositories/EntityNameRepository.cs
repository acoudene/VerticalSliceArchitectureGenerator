// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Feature.Data.MongoDb.Repositories;

public class EntityNameRepository : IEntityNameRepository
{
  public const string CollectionName = "entityName";

  protected TimeStampedMongoRepositoryBehavior<EntityName, EntityNameMongo> Behavior { get => _behavior; }
  private readonly TimeStampedMongoRepositoryBehavior<EntityName, EntityNameMongo> _behavior;

  public EntityNameRepository(IMongoContext mongoContext)
  {
    _behavior = new TimeStampedMongoRepositoryBehavior<EntityName, EntityNameMongo>(mongoContext, CollectionName);
    _behavior.SetUniqueIndex(entity => entity.Id);
  }

  // This commented part could be used to have benefits of mongo entity typing
  //protected virtual EntityNameBase ToEntity(EntityNameMongoBase mongoEntity)
  //{
  //  return mongoEntity.ToInheritedEntity();
  //}

  // This commented part could be used to have benefits of mongo entity typing
  //protected virtual EntityNameMongoBase ToMongoEntity(EntityNameBase entity)
  //{
  //  return entity.ToInheritedMongo();
  //}

  protected virtual EntityName ToEntity(EntityNameMongo mongoEntity)
  {
    return mongoEntity.ToEntity();
  }

  protected virtual EntityNameMongo ToMongoEntity(EntityName entity)
  {
    return entity.ToMongo();
  }

  public virtual async Task<List<EntityName>> GetAllAsync(CancellationToken cancellationToken = default) 
    => await _behavior.GetAllAsync(ToEntity, cancellationToken);

  public virtual async Task<EntityName?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdAsync(id, ToEntity, cancellationToken);

  public virtual async Task<List<EntityName>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdsAsync(ids, ToEntity, cancellationToken);

  public virtual async Task CreateAsync(EntityName newItem, CancellationToken cancellationToken = default) 
    => await _behavior.CreateAsync(newItem, ToMongoEntity, cancellationToken);

  public virtual async Task UpdateAsync(EntityName updatedItem, CancellationToken cancellationToken = default) 
    => await _behavior.UpdateAsync(updatedItem, ToMongoEntity, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual void SetUniqueIndex(params Expression<Func<EntityNameMongo, object>>[] fields)
    => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
    => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params IndexKeysDefinition<EntityNameMongo>[] fields)
    => _behavior.SetUniqueIndex(fields);
}
