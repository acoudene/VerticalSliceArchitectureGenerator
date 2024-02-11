// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api;

public abstract class RestControllerBase<TDto, TEntity, TRepository> : ControllerBase
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity>
{
  private readonly RestBehavior<TDto, TEntity, TRepository> _restBehavior;

  protected RestBehavior<TDto, TEntity, TRepository> RestBehavior { get => _restBehavior; }

  public RestControllerBase(RestBehavior<TDto, TEntity, TRepository> restBehavior)
  {
    _restBehavior = restBehavior ?? throw new ArgumentNullException(nameof(restBehavior));
  }

  public RestControllerBase(TRepository repository)
    : this(new RestBehavior<TDto, TEntity, TRepository>(repository))
  { }

  protected abstract TEntity ToEntity(TDto dto);
  protected abstract TDto ToDto(TEntity entity);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetAllAsync()
  {
    return await _restBehavior.GetAllAsync(ToDto);
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> GetByIdAsync(Guid id)
  {
    return await _restBehavior.GetByIdAsync(id, ToDto);
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, NotFound, ProblemHttpResult>> GetByIdsAsync([FromQuery] List<Guid> ids)
  {
    return await _restBehavior.GetByIdsAsync(ids, ToDto);
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, BadRequest, ProblemHttpResult>> CreateAsync([FromBody] TDto newDto)
  {
    return await _restBehavior.CreateAsync(newDto, ToEntity);
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, BadRequest, NotFound, ProblemHttpResult>> UpdateAsync(Guid id, [FromBody] TDto updatedDto)
  {
    return await _restBehavior.UpdateAsync(id, updatedDto, ToEntity);
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> DeleteAsync(Guid id)
  {
    return await _restBehavior.DeleteAsync(id, ToDto);
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<TDto> patchDto)
  {
    return await _restBehavior.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto);
  }
}
