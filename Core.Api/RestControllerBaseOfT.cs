using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace $safeprojectname$;

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

  [HttpGet("{id:length(36)}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, ProblemHttpResult>> GetAsync(Guid id)
  {
    var entity = await _repository.GetAsync(id);

    if (entity is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(ToDto(entity));
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, ProblemHttpResult>> CreateAsync([FromBody] TDto newDto)
  {
    var toCreateEntity = ToEntity(newDto);
    await _repository.CreateAsync(toCreateEntity);

    string? location = Url.Action("get", new { id = newDto.Id });
    if (string.IsNullOrWhiteSpace(location))
      throw new InvalidOperationException($"Can't create {nameof(location)} url");
    return TypedResults.Created(location, newDto);
  }

  [HttpPut]
  public virtual async Task<Results<NoContent, NotFound, ProblemHttpResult>> UpdateAsync([FromBody] TDto updatedDto)
  {
    Guid id = updatedDto.Id;

    var existingEntity = await _repository.GetAsync(id);
    if (existingEntity is null)
    {
      return TypedResults.NotFound();
    }

    var toUpdateEntity = ToEntity(updatedDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return TypedResults.NoContent();
  }

  [HttpDelete("{id:length(36)}")]
  public virtual async Task<Results<NoContent, NotFound, ProblemHttpResult>> DeleteAsync(Guid id)
  {
    var item = await _repository.GetAsync(id);

    if (item is null)
    {
      return TypedResults.NotFound();
    }

    await _repository.RemoveAsync(id);

    return TypedResults.NoContent();
  }

  [HttpPatch]
  public virtual async Task<ActionResult<TDto>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<TDto> patchDto)
  {
    if (patchDto == null)
    {
      return BadRequest(ModelState);
    }

    var existingEntity = await _repository.GetAsync(id);
    if (existingEntity is null)
    {
      return NotFound();
    }

    var toUpdateDto = ToDto(existingEntity);
    patchDto.ApplyTo(toUpdateDto, ModelState);

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var toUpdateEntity = ToEntity(toUpdateDto);
    await _repository.UpdateAsync(toUpdateEntity);

    return toUpdateDto;
  }
}
