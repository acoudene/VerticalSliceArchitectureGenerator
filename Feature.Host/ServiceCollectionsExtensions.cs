// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

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

  public static void ConfigureDataAdapters(this IServiceCollection serviceCollection, IConfiguration configuration, string? forcedConnectionString)
  {
    /// Connexion strings
    serviceCollection.Configure<DatabaseSettings>(configuration);
    serviceCollection.Configure<DatabaseSettings>(options => options.ConnectionString = forcedConnectionString ?? options.ConnectionString);

    AddDataAdapters(serviceCollection);
  }
}
