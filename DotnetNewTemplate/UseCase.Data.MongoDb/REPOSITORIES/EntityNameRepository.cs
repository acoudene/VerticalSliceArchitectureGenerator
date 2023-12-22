﻿namespace UseCase.Data.MongoDb.Repositories;

public class EntityNameRepository : MongoRepositoryBase<EntityName, EntityNameMongo>
  , IEntityNameRepository
{
  public const string CollectionName = "EntityName";

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
