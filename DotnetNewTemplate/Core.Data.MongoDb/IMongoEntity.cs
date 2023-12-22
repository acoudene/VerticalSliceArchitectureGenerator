using MongoDB.Bson;

namespace Core.Data.MongoDb;

public interface IMongoEntity : IEntity
{
  ObjectId ObjectId { get; set; }
}
