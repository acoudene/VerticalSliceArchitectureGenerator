using MongoDB.Driver;

namespace Core.Data.MongoDb;

public interface IMongoContext
{
    IMongoDatabase GetDatabase();
}