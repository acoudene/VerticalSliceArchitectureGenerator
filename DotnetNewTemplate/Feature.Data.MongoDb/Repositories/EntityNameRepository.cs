// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Feature.Data.MongoDb.Repositories;

public class EntityNameRepository : IEntityNameRepository
{
  public const string CollectionName = "entityName";

  protected MongoRepositoryComponent<EntityName, EntityNameMongo> MongoRepositoryComponent { get => _mongoRepositoryComponent; }
  private readonly MongoRepositoryComponent<EntityName, EntityNameMongo> _mongoRepositoryComponent;

  public EntityNameRepository(IMongoContext mongoContext)
  {
    _mongoRepositoryComponent = new MongoRepositoryComponent<EntityName, EntityNameMongo>(mongoContext, CollectionName);
    _mongoRepositoryComponent.SetUniqueIndex(entity => entity.Id);
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

  public virtual async Task<List<EntityName>> GetAllAsync() => await _mongoRepositoryComponent.GetAllAsync(ToEntity);

  public virtual async Task<EntityName?> GetByIdAsync(Guid id) => await _mongoRepositoryComponent.GetByIdAsync(id, ToEntity);

  public virtual async Task<List<EntityName>> GetByIdsAsync(List<Guid> ids) => await _mongoRepositoryComponent.GetByIdsAsync(ids, ToEntity);

  public virtual async Task CreateAsync(EntityName newItem) => await _mongoRepositoryComponent.CreateAsync(newItem, ToMongoEntity);

  public virtual async Task UpdateAsync(EntityName updatedItem) => await _mongoRepositoryComponent.UpdateAsync(updatedItem, ToMongoEntity);

  public virtual async Task RemoveAsync(Guid id) => await _mongoRepositoryComponent.RemoveAsync(id);

  public virtual void SetUniqueIndex(params Expression<Func<EntityNameMongo, object>>[] fields)
    => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
    => _mongoRepositoryComponent.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params IndexKeysDefinition<EntityNameMongo>[] fields)
    => _mongoRepositoryComponent.SetUniqueIndex(fields);
}
