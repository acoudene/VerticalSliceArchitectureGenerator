// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core.Api;

public abstract class RestControllerBase<TDto, TEntity, TRepository> : ControllerBase
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity>
{
  private readonly ILogger<RestControllerBase<TDto, TEntity, TRepository>> _logger;
  private readonly RestComponent<TDto, TEntity, TRepository> _restComponent;

  protected RestComponent<TDto, TEntity, TRepository> RestComponent { get => _restComponent; }

  public RestControllerBase(
    ILogger<RestControllerBase<TDto, TEntity, TRepository>> logger, 
    RestComponent<TDto, TEntity, TRepository> restComponent)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _restComponent = restComponent ?? throw new ArgumentNullException(nameof(restComponent));
  }

  public RestControllerBase(ILogger<RestControllerBase<TDto, TEntity, TRepository>> logger, TRepository repository)
    : this(logger, new RestComponent<TDto, TEntity, TRepository>(repository))
  { }

  protected abstract TEntity ToEntity(TDto dto);
  protected abstract TDto ToDto(TEntity entity);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetAllAsync()
  {
    try
    {
      return TypedResults.Ok(await _restComponent.GetAllAsync(ToDto));
    }
    catch(ArgumentException ex) 
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch(Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(Guid id)
  {
    try
    {
      var foundEntity = await _restComponent.GetByIdAsync(id, ToDto);
      if (foundEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(foundEntity);
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetByIdsAsync([FromQuery] List<Guid> ids)
  {
    try
    {
      return TypedResults.Ok(await _restComponent.GetByIdsAsync(ids, ToDto));
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }    
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, BadRequest, ProblemHttpResult>> CreateAsync([FromBody] TDto newDto)
  {
    try
    {      
      return TypedResults.Created("{newDto.Id}", await _restComponent.CreateAsync(newDto, ToEntity));
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, BadRequest, ProblemHttpResult>> UpdateAsync(
    Guid id, 
    [FromBody] TDto updatedDto)
  {
    try
    {
      var updatedEntity = await _restComponent.UpdateAsync(id, updatedDto, ToEntity);
      if (updatedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.NoContent();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(Guid id)
  {
    try
    {
      var deletedEntity = await _restComponent.DeleteAsync(id, ToDto);
      if (deletedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(deletedEntity);
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id, 
    [FromBody] JsonPatchDocument<TDto> patchDto)
  {
    try
    {
      var patchedEntity = await _restComponent.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto);
      if (patchedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(patchedEntity);
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }
}
