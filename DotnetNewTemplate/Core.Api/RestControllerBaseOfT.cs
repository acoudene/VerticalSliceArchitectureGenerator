// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Core.Data;
using Core.Dtos;

namespace Core.Api;

public abstract class RestControllerBase<TDto, TEntity, TRepository>  : ControllerBase
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity> 
{
  private readonly TRepository _repository;

  protected TRepository Repository { get => _repository; }

  public RestControllerBase(TRepository repository)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
  }

  protected abstract TEntity ToEntity(TDto dto);
  protected abstract TDto ToDto(TEntity entity);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TDto>>, ProblemHttpResult>> GetAllAsync()
  {
    var entities = await _repository.GetAllAsync();
    return TypedResults.Ok(entities
      .Select(entity => ToDto(entity))
      .ToList());
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, ProblemHttpResult>> GetByIdAsync(Guid id)
  {
    var entity = await _repository.GetByIdAsync(id);

    if (entity is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(ToDto(entity));
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TDto>>, NotFound, ProblemHttpResult>> GetByIdsAsync([FromQuery] List<Guid> ids)
  {
    var entities = await _repository.GetByIdsAsync(ids);
    return TypedResults.Ok(entities
      .Select(entity => ToDto(entity))
      .ToList());
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, ProblemHttpResult>> CreateAsync([FromBody] TDto newDto)
  {
    var toCreateEntity = ToEntity(newDto);
    await _repository.CreateAsync(toCreateEntity);
    return TypedResults.Created("{newDto.Id}", newDto);
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, BadRequest, NotFound, ProblemHttpResult>> UpdateAsync(Guid id, [FromBody] TDto updatedDto)
  {
    if (id != updatedDto.Id)
      return TypedResults.BadRequest();    

    var existingEntity = await _repository.GetByIdAsync(id);
    if (existingEntity is null)
      return TypedResults.NotFound();

    var toUpdateEntity = ToEntity(updatedDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return TypedResults.NoContent();
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteAsync(Guid id)
  {
    var item = await _repository.GetByIdAsync(id);

    if (item is null)
    {
      return TypedResults.NotFound();
    }

    await _repository.RemoveAsync(id);

    return TypedResults.NoContent();
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<TDto> patchDto)
  {
    if (patchDto == null)
    {
      return TypedResults.BadRequest();
    }

    var existingEntity = await _repository.GetByIdAsync(id);
    if (existingEntity is null)
    {
      return TypedResults.NotFound();
    }

    var toUpdateDto = ToDto(existingEntity);
    patchDto.ApplyTo(toUpdateDto, ModelState);

    if (!ModelState.IsValid)
    {
      return TypedResults.BadRequest();
    }

    var toUpdateEntity = ToEntity(toUpdateDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return TypedResults.Ok(toUpdateDto);
  }
}
