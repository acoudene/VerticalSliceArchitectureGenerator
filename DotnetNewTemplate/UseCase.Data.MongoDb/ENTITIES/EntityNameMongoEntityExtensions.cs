namespace UseCase.Data.MongoDb.Entities;

public static class EntityNameMongoEntityExtensions
{
  public static EntityNameMongo ToMongo(this EntityName entity)
  {
    return new EntityNameMongo()
    {
      Id = entity.Id
    };
  }

  public static EntityName ToEntity(this EntityNameMongo mongoEntity)
  {
    return new EntityName()
    {
      Id = mongoEntity.Id
    };
  }
}
