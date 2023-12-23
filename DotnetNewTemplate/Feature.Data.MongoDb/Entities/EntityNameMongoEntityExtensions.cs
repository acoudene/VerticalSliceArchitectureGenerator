namespace UseCase.Data.MongoDb.Entities;

public static class EntityNameMongoEntityExtensions
{
  public static EntityNameMongo ToMongo(this EntityName entity)
  {
    return new EntityNameMongo()
    {
      Id = entity.Id

      // TODO - EntityMapping - Business Entity to Mongo Entity to complete
    };
  }

  public static EntityName ToEntity(this EntityNameMongo mongoEntity)
  {
    return new EntityName()
    {
      Id = mongoEntity.Id

      // TODO - EntityMapping - Mongo Entity to Business Entity to complete
    };
  }
}
