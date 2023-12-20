namespace $safeprojectname$.Entities;

[BsonIgnoreExtraElements]
public record RequestFormMongo : IIdentifierMongoEntity
{
  [BsonId]
  [BsonElement("_id")]
  [BsonRepresentation(representation: BsonType.ObjectId)]
  [BsonIgnoreIfDefault]
  public ObjectId ObjectId { get; set; }

  [BsonElement("uuid")]
  [BsonGuidRepresentation(GuidRepresentation.Standard)]
  public required Guid Id { get; set; }

  private readonly FormPropertiesTemplateMongo _formPropertiesTemplate;

  [BsonElement("formPropertiesTemplate")]
  public FormPropertiesTemplateMongo FormPropertiesTemplate => _formPropertiesTemplate;

  public RequestFormMongo(FormPropertiesTemplateMongo formPropertiesTemplate)
  {
    _formPropertiesTemplate = formPropertiesTemplate ?? throw new ArgumentNullException(nameof(formPropertiesTemplate));
  }

  [BsonElement("fields")]
  public List<FieldMongoBase> Fields { get; set; } = Enumerable.Empty<FieldMongoBase>().ToList();
}