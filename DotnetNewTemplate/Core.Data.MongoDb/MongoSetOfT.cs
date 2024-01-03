// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public class MongoSet<TEntity> : IMongoSet<TEntity> where TEntity : IIdentifierMongoEntity
{
    private readonly string _collectionName;
    private readonly IMongoContext _mongoContext;

    public string GetCollectionName() => _collectionName;

    public MongoSet(IMongoContext mongoContext, string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (mongoContext is null)
            throw new ArgumentNullException(nameof(mongoContext));

        _collectionName = collectionName;
        _mongoContext = mongoContext;
    }

    public IMongoCollection<TEntity> GetCollection()
    {
        var database = _mongoContext.GetDatabase();
        if (database is null)
            throw new InvalidOperationException($"No MongoDb database for {_collectionName}");

        return database.GetCollection<TEntity>(_collectionName);
    }

    public async Task<List<TEntity>> GetAllAsync() =>
      await GetCollection()
      .Find(_ => true)
      .ToListAsync();

    public async Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter) =>
      await GetCollection()
      .Find(filter)
      .FirstOrDefaultAsync();

    public async Task CreateAsync(TEntity newItem) =>
      await GetCollection()
      .InsertOneAsync(newItem);

    public async Task UpdateAsync(Expression<Func<TEntity, bool>> filter, TEntity updatedItem)
    {
        await GetCollection()
          .ReplaceOneAsync(filter, updatedItem);
    }

    public async Task RemoveAsync(Expression<Func<TEntity, bool>> filter) =>
      await GetCollection()
      .DeleteOneAsync(filter);
}