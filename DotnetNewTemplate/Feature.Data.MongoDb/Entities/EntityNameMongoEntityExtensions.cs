// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Data.MongoDb.Entities;

public static class EntityNameMongoEntityExtensions
{
  // This commented part could be used to have benefits of mongo entity typing
  //public static EntityNameMongoBase ToInheritedMongo(this EntityNameBase entity)
  //{
  //  switch (entity)
  //  {
  //    case EntityNameInherited inheritedEntity: return inheritedEntity.ToMongo();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  // This commented part could be used to have benefits of mongo entity typing
  //public static EntityNameBase ToInheritedEntity(this EntityNameMongoBase mongoEntity)
  //{
  //  switch (mongoEntity)
  //  {
  //    case EntityNameInheritedMongo inheritedMongo: return inheritedMongo.ToEntity();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

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
