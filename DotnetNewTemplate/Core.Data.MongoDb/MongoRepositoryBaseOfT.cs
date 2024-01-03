// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public abstract class MongoRepositoryBase<TEntity, TMongoEntity> : IRepository<TEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  protected IMongoContext MongoContext { get => _mongoContext; }
  protected IMongoSet<TMongoEntity> MongoSet { get => _mongoSet; }

  private readonly IMongoContext _mongoContext;
  private readonly IMongoSet<TMongoEntity> _mongoSet;

  public MongoRepositoryBase(IMongoContext mongoContext, string collectionName)
  {
    if (mongoContext is null)
      throw new ArgumentNullException(nameof(mongoContext));
    if (string.IsNullOrWhiteSpace(collectionName))
      throw new ArgumentNullException(nameof(collectionName));

    _mongoContext = mongoContext;
    _mongoSet = new MongoSet<TMongoEntity>(mongoContext, collectionName);

    SetUniqueIndex(entity => entity.Id);
  }

  protected abstract TEntity MapToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity MapToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync() => (await _mongoSet.GetAllAsync())
    .OfType<TMongoEntity>()
    .Select(mongoEntity => MapToEntity(mongoEntity))
    .ToList();

  public virtual async Task<TEntity?> GetByIdAsync(Guid id)
  {
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    var mongoEntity = await _mongoSet.FindFirstOrDefaultAsync(x => x.Id == id);
    if (mongoEntity is null)
      return default;

    return MapToEntity(mongoEntity);
  }

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids)
  {
    // db.getCollection("<CollectionName>").find({id: {$in: [UUID("3FA85F64-5717-4562-B3FC-2C963F66AFA1"),UUID("3FA85F64-5717-4562-B3FC-2C963F66AFA2")]}})

    var filter = Builders<TMongoEntity>.Filter.In(f => f.Id, ids);

    return (await MongoSet
      .GetCollection()
      .Find(filter)
      .ToListAsync())
      .Select(mongoEntity => MapToEntity(mongoEntity))
      .ToList();
  }

  public virtual async Task CreateAsync(TEntity newItem)
  {
    if (newItem is null)
      throw new ArgumentNullException(nameof(newItem));

    Guid id = newItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    await _mongoSet.CreateAsync(MapToMongoEntity(newItem));
  }


  public virtual async Task UpdateAsync(TEntity updatedItem)
  {
    if (updatedItem is null)
      throw new ArgumentNullException(nameof(updatedItem));

    Guid id = updatedItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    await _mongoSet.UpdateAsync(x => x.Id == id, MapToMongoEntity(updatedItem));
  }

  public virtual async Task RemoveAsync(Guid id)
  {
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    await _mongoSet.RemoveAsync(x => x.Id == id);
  }

  public virtual void SetUniqueIndex(Expression<Func<TMongoEntity, object>> field)
  {
    var indexKeysDefinition = Builders<TMongoEntity>.IndexKeys
      .Ascending(field);

    var indexModel = new CreateIndexModel<TMongoEntity>(indexKeysDefinition, new CreateIndexOptions() { Unique = true });

    MongoSet.GetCollection()
      .Indexes
      .CreateOne(indexModel);
  }
}
