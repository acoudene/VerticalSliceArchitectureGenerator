﻿// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Core.Proxying;

namespace Core.Presentation;

public class RestViewModelComponent<TViewObject, TDto>
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
{
  private readonly IRestClient<TDto> _restClient;

  public RestViewModelComponent(IRestClient<TDto> restClient)
  {
    _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
  }

  public virtual async Task CreateAsync(
    TViewObject newItem,
    Func<TViewObject, TDto> toDtoFunc,
    CancellationToken cancellationToken = default)
  {
    if (toDtoFunc is null)
      throw new ArgumentNullException(nameof(toDtoFunc));

    await _restClient.CreateAsync(toDtoFunc(newItem), cancellationToken);
  }

  public virtual async Task<List<TViewObject>> GetAllAsync(
    Func<TDto, TViewObject> toViewFunc,
    CancellationToken cancellationToken = default)
  {
    if (toViewFunc is null)
      throw new ArgumentNullException(nameof(toViewFunc));

    return (await _restClient
      .GetAllAsync(cancellationToken))
      .Select(dto => toViewFunc(dto))
      .ToList();
  }

  public virtual async Task<TViewObject?> GetByIDAsync(
    Guid id,
    Func<TDto, TViewObject> toViewFunc,
    CancellationToken cancellationToken = default)
  {
    if (toViewFunc is null)
      throw new ArgumentNullException(nameof(toViewFunc));

    return toViewFunc((await _restClient
      .GetByIdAsync(id, cancellationToken)));

  }

  public virtual async Task<List<TViewObject>?> GetByIdsAsync(
    List<Guid> ids,
    Func<TDto, TViewObject> toViewFunc,
    CancellationToken cancellationToken = default)
  {
    if (toViewFunc is null)
      throw new ArgumentNullException(nameof(toViewFunc));

    return (await _restClient
      .GetByIdsAsync(ids, cancellationToken))
      .Select(dto => toViewFunc(dto))
      .ToList();
  }

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
  {
    await _restClient.DeleteAsync(id, cancellationToken);
  }

  public virtual async Task UpdateAsync(
    Guid id,
    TViewObject updatedItem,
    Func<TViewObject, TDto> toDtoFunc,
    CancellationToken cancellationToken = default)
  {
    if (toDtoFunc is null)
      throw new ArgumentNullException(nameof(toDtoFunc));

    await _restClient.UpdateAsync(id, toDtoFunc(updatedItem), cancellationToken);
  }
}
