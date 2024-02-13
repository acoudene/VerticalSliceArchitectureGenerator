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
  private readonly RestComponent<TDto, TEntity, TRepository> _restComponent;

  protected RestComponent<TDto, TEntity, TRepository> RestComponent { get => _restComponent; }

  public RestControllerBase(RestComponent<TDto, TEntity, TRepository> restComponent)
  {
    _restComponent = restComponent ?? throw new ArgumentNullException(nameof(restComponent));
  }

  public RestControllerBase(TRepository repository)
    : this(new RestComponent<TDto, TEntity, TRepository>(repository))
  { }

  protected abstract TEntity ToEntity(TDto dto);
  protected abstract TDto ToDto(TEntity entity);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetAllAsync()
  {
    return await _restComponent.GetAllAsync(ToDto);
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> GetByIdAsync(Guid id)
  {
    return await _restComponent.GetByIdAsync(id, ToDto);
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, NotFound, ProblemHttpResult>> GetByIdsAsync([FromQuery] List<Guid> ids)
  {
    return await _restComponent.GetByIdsAsync(ids, ToDto);
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, BadRequest, ProblemHttpResult>> CreateAsync([FromBody] TDto newDto)
  {
    return await _restComponent.CreateAsync(newDto, ToEntity);
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, BadRequest, NotFound, ProblemHttpResult>> UpdateAsync(Guid id, [FromBody] TDto updatedDto)
  {
    return await _restComponent.UpdateAsync(id, updatedDto, ToEntity);
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> DeleteAsync(Guid id)
  {
    return await _restComponent.DeleteAsync(id, ToDto);
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TDto>, BadRequest, NotFound, ProblemHttpResult>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<TDto> patchDto)
  {
    return await _restComponent.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto);
  }
}
