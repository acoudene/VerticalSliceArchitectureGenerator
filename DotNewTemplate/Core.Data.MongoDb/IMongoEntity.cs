using MongoDB.Bson;

namespace $safeprojectname$;

public interface IMongoEntity : IEntity
{
  ObjectId ObjectId { get; set; }
}
