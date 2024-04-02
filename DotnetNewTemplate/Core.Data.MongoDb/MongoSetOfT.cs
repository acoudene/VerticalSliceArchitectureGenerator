// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public class MongoSet<TMongoEntity> : IMongoSet<TMongoEntity> where TMongoEntity : IIdentifierMongoEntity
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

  public IMongoCollection<TMongoEntity> GetCollection()
  {
    var database = _mongoContext.GetDatabase();
    if (database is null)
      throw new InvalidOperationException($"No MongoDb database for {_collectionName}");

    return database
      .GetCollection<TMongoEntity>(_collectionName)
      .OfType<TMongoEntity>();
  }

  public async Task<List<TMongoEntity>> GetAllAsync() =>
    await GetCollection()
    .Find(_ => true)
    .ToListAsync();

  public async Task<TMongoEntity?> GetByFilterAsync(Expression<Func<TMongoEntity, bool>> filter) =>
    await GetCollection()
    .Find(filter)
    .FirstOrDefaultAsync();

  public async Task<List<TMongoEntity>> GetItemsByFilterAsync(Expression<Func<TMongoEntity, bool>> filter) =>
    await GetCollection()
    .Find(filter)
    .ToListAsync();

  public async Task<List<TMongoEntity>> GetItemsInAsync<TField>(Expression<Func<TMongoEntity, TField>> field, IEnumerable<TField> values)
  {
    var filter = Builders<TMongoEntity>.Filter.In(field, values);

    return (await GetCollection()
      .Find(filter)
      .ToListAsync());
  }

  public async Task CreateAsync(TMongoEntity newItem) =>
    await GetCollection()
    .InsertOneAsync(newItem);

  public async Task UpdateAsync(Expression<Func<TMongoEntity, bool>> filter, TMongoEntity updatedItem)
  {
    await GetCollection()
      .ReplaceOneAsync(filter, updatedItem);
  }

  public async Task RemoveAsync(Expression<Func<TMongoEntity, bool>> filter) =>
    await GetCollection()
    .DeleteOneAsync(filter);
}