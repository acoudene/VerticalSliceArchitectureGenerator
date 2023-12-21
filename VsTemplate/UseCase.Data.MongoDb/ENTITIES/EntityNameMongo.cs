namespace $safeprojectname$.Entities;

[BsonIgnoreExtraElements]
public record $ext_entityName$Mongo : IIdentifierMongoEntity
{
  [BsonId]
  [BsonElement("_id")]
  [BsonRepresentation(representation: BsonType.ObjectId)]
  [BsonIgnoreIfDefault]
  public ObjectId ObjectId { get; set; }

  [BsonElement("uuid")]
  [BsonGuidRepresentation(GuidRepresentation.Standard)]
  public required Guid Id { get; set; }  
}