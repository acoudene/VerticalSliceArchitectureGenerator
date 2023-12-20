using MongoDB.Driver;

namespace $safeprojectname$;

public interface IMongoContext
{
    IMongoDatabase GetDatabase();
}