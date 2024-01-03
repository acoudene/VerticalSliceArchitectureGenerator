// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
  Task<List<TEntity>> GetAllAsync();

  Task<TEntity?> GetAsync(Guid id);

  Task<List<TEntity>?> GetAsync(List<Guid> ids);

  Task CreateAsync(TEntity newItem);

  Task UpdateAsync(TEntity updatedItem);
  
  Task RemoveAsync(Guid id);
}