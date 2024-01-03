// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public interface IMongoSet<TEntity> where TEntity : IIdentifierMongoEntity
{
    IMongoCollection<TEntity> GetCollection();

    Task<List<TEntity>> GetAllAsync();

    Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

    Task CreateAsync(TEntity newItem);

    Task UpdateAsync(Expression<Func<TEntity, bool>> filter, TEntity updatedItem);

    Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
}