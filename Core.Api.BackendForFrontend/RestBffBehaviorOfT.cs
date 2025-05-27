// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Core.Proxying;
using Core.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Api.BackendForFrontend;

/// <summary>
/// Component used to manage proxy interaction with a backend API
/// </summary>
/// <typeparam name="TViewObject"></typeparam>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TClient"></typeparam>
public class RestBffBehavior<TViewObject, TDto, TClient>
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
  where TClient : IRestClient<TDto>
{
  private readonly TClient _client;

  public TClient Client { get => _client; }

  public RestBffBehavior(TClient client)
  {
    _client = client ?? throw new ArgumentNullException(nameof(client));
  }

  public virtual async Task<List<TViewObject>> GetAllAsync(
    Func<TDto, TViewObject> toVoFunc, 
    CancellationToken cancellationToken = default)
  {
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var dtos = await _client.GetAllAsync(cancellationToken);

    return dtos
      .Select(dto => toVoFunc(dto))
      .ToList();
  }

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var dto = await _client.GetByIdAsync(id, cancellationToken);
    if (dto is null)
      return null;

    return toVoFunc(dto);
  }

  public virtual async Task<List<TViewObject>> GetByIdsAsync(List<Guid> ids, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (ids is null) throw new ArgumentNullException(nameof(ids));
    if (!ids.Any()) throw new ArgumentOutOfRangeException(nameof(ids));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var dtos = await _client.GetByIdsAsync(ids, cancellationToken);

    return dtos
      .Select(entity => toVoFunc(entity))
      .ToList();
  }

  public virtual async Task<TViewObject> CreateAsync(TViewObject newVo, Func<TViewObject, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (newVo is null) throw new ArgumentNullException(nameof(newVo));
    if (newVo.Id == Guid.Empty) throw new ArgumentNullException(nameof(newVo.Id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var toCreateDto = toDtoFunc(newVo);

    await _client.CreateAsync(toCreateDto, cancellationToken);

    return newVo; // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject> CreateOrUpdateAsync(TViewObject newOrToUpdateVo, Func<TViewObject, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (newOrToUpdateVo is null) throw new ArgumentNullException(nameof(newOrToUpdateVo));
    if (newOrToUpdateVo.Id == Guid.Empty) throw new ArgumentNullException(nameof(newOrToUpdateVo.Id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var toCreateOrUpdateDto = toDtoFunc(newOrToUpdateVo);

    await _client.CreateOrUpdateAsync(toCreateOrUpdateDto, cancellationToken);

    return newOrToUpdateVo; // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> UpdateAsync(Guid id, TViewObject updatedVo, Func<TViewObject, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (id != updatedVo.Id) throw new ArgumentOutOfRangeException(nameof(updatedVo.Id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var existingDto = await _client.GetByIdAsync(id, cancellationToken);
    if (existingDto is null)
      return null;

    var toUpdateDto = toDtoFunc(updatedVo);
    await _client.UpdateAsync(id, toUpdateDto, cancellationToken);

    return updatedVo; // Don't read the updated value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> DeleteAsync(Guid id, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var beforeRemoveDto = await _client.GetByIdAsync(id, cancellationToken);
    if (beforeRemoveDto is null)
      return null;

    await _client.DeleteAsync(id, cancellationToken);

    return toVoFunc(beforeRemoveDto); // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> PatchAsync(
    Guid id,
    JsonPatchDocument<TViewObject> patchDto,
    ModelStateDictionary modelState,
    Func<TViewObject, TDto> toDtoFunc,
    Func<TDto, TViewObject> toVoFunc,
    CancellationToken cancellationToken = default)
  {
    if (patchDto is null) throw new ArgumentNullException(nameof(patchDto));
    if (modelState is null) throw new ArgumentNullException(nameof(modelState));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var existingDto = await _client.GetByIdAsync(id, cancellationToken);
    if (existingDto is null)
      return null;

    var toUpdateVo = toVoFunc(existingDto);
    patchDto.ApplyTo(toUpdateVo, modelState);

    if (!modelState.IsValid)
      throw new ArgumentOutOfRangeException(nameof(modelState));

    var toUpdateDto = toDtoFunc(toUpdateVo);
    await _client.UpdateAsync(id, toUpdateDto, cancellationToken);

    return toUpdateVo; // Don't read the inserted value because the Dto should be read and checked in the API.
  }
}
