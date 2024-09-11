// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data.MongoDb;

public class TimeStampedMongoRepositoryComponent<TEntity, TMongoEntity> : MongoRepositoryComponent<TEntity, TMongoEntity>
  where TEntity : IIdentifierEntity, ITimestampedEntity
  where TMongoEntity : IIdentifierMongoEntity, ITimestampedMongoEntity
{
  public TimeStampedMongoRepositoryComponent(IMongoContext mongoContext, string collectionName)
    : base(mongoContext, collectionName)
  {
  }

  public override async Task CreateAsync(TEntity newItem, Func<TEntity, TMongoEntity> toMongoEntityFunc)
  {
    if (newItem is null)
      throw new ArgumentNullException(nameof(newItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = newItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    newItem.CreatedAt = DateTime.UtcNow;
    newItem.UpdatedAt = DateTime.UtcNow;

    await base.CreateAsync(newItem, toMongoEntityFunc);
  }


  public override async Task UpdateAsync(TEntity updatedItem, Func<TEntity, TMongoEntity> toMongoEntityFunc)
  {
    if (updatedItem is null)
      throw new ArgumentNullException(nameof(updatedItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = updatedItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    updatedItem.UpdatedAt = DateTime.UtcNow;

    await base.UpdateAsync(updatedItem, toMongoEntityFunc);
  }
}
