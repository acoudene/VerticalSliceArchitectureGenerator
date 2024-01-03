// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace UseCase.Data.MongoDb.Repositories;

public class EntityNameRepository : MongoRepositoryBase<EntityName, EntityNameMongo>
  , IEntityNameRepository
{
  public const string CollectionName = "entityName";

  public EntityNameRepository(IMongoContext mongoContext)
    : base(mongoContext, CollectionName)
  { 
  }

  protected override EntityName MapToEntity(EntityNameMongo mongoEntity)
  {
    return mongoEntity.ToEntity();
  }

  protected override EntityNameMongo MapToMongoEntity(EntityName entity)
  {
    return entity.ToMongo();
  }
}
