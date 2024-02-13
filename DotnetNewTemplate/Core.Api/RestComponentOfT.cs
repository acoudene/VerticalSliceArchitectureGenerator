// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Api;

public class RestComponent<TDto, TEntity, TRepository>
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity> 
{
  private readonly TRepository _repository;
  
  public TRepository Repository { get => _repository; }

  public RestComponent(TRepository repository)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
  }

  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetAllAsync(Func<TEntity, TDto> toDtoFunc)
  {
    if (toDtoFunc is null)
      return TypedResults.BadRequest();

    var entities = await _repository.GetAllAsync();

    return TypedResults.Ok(entities
      .Select(entity => toDtoFunc(entity))
      .ToList());
  }

  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> GetByIdAsync(
    Guid id, 
    Func<TEntity, TDto> toDtoFunc)
  {
    if (id == Guid.Empty || toDtoFunc is null)
      return TypedResults.BadRequest();

    var entity = await _repository.GetByIdAsync(id);
    if (entity is null)
      return TypedResults.NotFound();    

    return TypedResults.Ok(toDtoFunc(entity));
  }

  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, NotFound, ProblemHttpResult>> GetByIdsAsync(
    List<Guid> ids, 
    Func<TEntity, TDto> toDtoFunc)
  {
    if (ids is null || toDtoFunc is null)
      return TypedResults.BadRequest();

    var entities = await _repository.GetByIdsAsync(ids);
    
    return TypedResults.Ok(entities
      .Select(entity => toDtoFunc(entity))
      .ToList());
  }

  public virtual async Task<Results<Created<TDto>, BadRequest, ProblemHttpResult>> CreateAsync(
    TDto newDto,
    Func<TDto, TEntity> toEntityFunc)
  {
    if ( newDto is null || toEntityFunc is null)
      return TypedResults.BadRequest();

    var toCreateEntity = toEntityFunc(newDto);
    
    await _repository.CreateAsync(toCreateEntity);
    
    return TypedResults.Created("{newDto.Id}", newDto);
  }

  public virtual async Task<Results<NoContent, BadRequest, NotFound, ProblemHttpResult>> UpdateAsync(
    Guid id, 
    TDto updatedDto,
    Func<TDto, TEntity> toEntityFunc)
  {    
    if (id != updatedDto.Id || toEntityFunc is null)
      return TypedResults.BadRequest();

    var existingEntity = await _repository.GetByIdAsync(id);
    if (existingEntity is null)
      return TypedResults.NotFound();

    var toUpdateEntity = toEntityFunc(updatedDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return TypedResults.NoContent();
  }

  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> DeleteAsync(
    Guid id,
    Func<TEntity, TDto> toDtoFunc)
  {    
    if (id == Guid.Empty || toDtoFunc is null)
      return TypedResults.BadRequest();

    var beforeRemoveEntity = await _repository.GetByIdAsync(id);
    if (beforeRemoveEntity is null)
      return TypedResults.NotFound();

    await _repository.RemoveAsync(id);

    return TypedResults.Ok(toDtoFunc(beforeRemoveEntity));
  }

  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> PatchAsync(
    Guid id, 
    JsonPatchDocument<TDto> patchDto,
    ModelStateDictionary modelState,
    Func<TDto, TEntity> toEntityFunc,
    Func<TEntity, TDto> toDtoFunc)
  {
    if (patchDto is null || modelState is null || toEntityFunc is null || toDtoFunc is null)    
      return TypedResults.BadRequest();    

    var existingEntity = await _repository.GetByIdAsync(id);
    if (existingEntity is null)    
      return TypedResults.NotFound();    

    var toUpdateDto = toDtoFunc(existingEntity);
    patchDto.ApplyTo(toUpdateDto, modelState);

    if (!modelState.IsValid)
      return TypedResults.BadRequest();    

    var toUpdateEntity = toEntityFunc(toUpdateDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return TypedResults.Ok(toUpdateDto);
  }
}
