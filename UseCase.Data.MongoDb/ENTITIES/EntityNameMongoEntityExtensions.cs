namespace $safeprojectname$.Entities;

public static class $ext_entityName$MongoEntityExtensions
{
  public static $ext_entityName$Mongo ToMongo(this $ext_entityName$ entity)
  {
    return new $ext_entityName$Mongo()
    {
      Id = entity.Id
    };
  }

  public static $ext_entityName$ ToEntity(this $ext_entityName$Mongo mongoEntity)
  {
    return new $ext_entityName$()
    {
      Id = mongoEntity.Id
    };
  }
}
