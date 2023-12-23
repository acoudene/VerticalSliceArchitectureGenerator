namespace Core.Presentation;

public interface IViewModel<TViewObject> : IViewModel where TViewObject : IViewObject
{
  Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default);

  Task<TViewObject?> GetAsync(Guid id, CancellationToken cancellationToken = default);

  Task<List<TViewObject>?> GetAsync(List<Guid> ids, CancellationToken cancellationToken = default);

  Task CreateAsync(TViewObject newItem, bool checkSuccessStatusCode = true, CancellationToken cancellationToken = default);

  Task UpdateAsync(TViewObject updatedItem, bool checkSuccessStatusCode = true, CancellationToken cancellationToken = default);

  Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
