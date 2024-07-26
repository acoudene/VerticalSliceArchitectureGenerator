using Core.Data.MongoDb;
using Feature.Data.MongoDb.Repositories;
using Feature.Data.Repositories;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Feature.Host;

public static class ServiceCollectionsExtensions
{
  public static void AddDataAdapters(this IServiceCollection serviceCollection)
  {
    // https://kevsoft.net/2022/02/18/setting-up-mongodb-to-use-standard-guids-in-csharp.html
#pragma warning disable CS0618
    BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
    //BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
    try
    {
      BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
    catch (BsonSerializationException)
    {
      // Just to let integration tests work
    }

    serviceCollection.AddScoped<IMongoContext, MongoContext>();
    serviceCollection.AddScoped<IEntityNameRepository, EntityNameRepository>();
  }

  public static void ConfigureDataAdapters(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    /// Connexion strings
    serviceCollection.Configure<DatabaseSettings>(configuration);

    AddDataAdapters(serviceCollection);
  }
}
