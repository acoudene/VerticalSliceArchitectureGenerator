namespace $safeprojectname$.Repositories;

public class $ext_entityName$Repository : MongoRepositoryBase<$ext_entityName$, $ext_entityName$Mongo>
  , I$ext_entityName$Repository
{
  public const string CollectionName = "$ext_entityName$";

  public $ext_entityName$Repository(IMongoContext mongoContext)
    : base(mongoContext, CollectionName)
  { 
  }

  protected override $ext_entityName$ MapToEntity($ext_entityName$Mongo mongoEntity)
  {
    return mongoEntity.ToEntity();
  }

  protected override $ext_entityName$Mongo MapToMongoEntity($ext_entityName$ entity)
  {
    return entity.ToMongo();
  }
}
