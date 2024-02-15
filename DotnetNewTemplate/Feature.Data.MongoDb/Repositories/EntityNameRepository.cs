// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Data.MongoDb.Repositories;

public class EntityNameRepository : MongoRepositoryBase<EntityName, EntityNameMongo>
  , IEntityNameRepository
{
  public const string CollectionName = "entityName";

  public EntityNameRepository(IMongoContext mongoContext)
    : base(mongoContext, CollectionName)
  { 
  }

  // This commented part could be used to have benefits of mongo entity typing
  //protected override EntityNameBase ToEntity(EntityNameMongoBase mongoEntity)
  //{
  //  return mongoEntity.ToInheritedEntity();
  //}

  // This commented part could be used to have benefits of mongo entity typing
  //protected override EntityNameMongoBase ToMongoEntity(EntityNameBase entity)
  //{
  //  return entity.ToInheritedMongo();
  //}

  protected override EntityName ToEntity(EntityNameMongo mongoEntity)
  {
    return mongoEntity.ToEntity();
  }

  protected override EntityNameMongo ToMongoEntity(EntityName entity)
  {
    return entity.ToMongo();
  }
}
