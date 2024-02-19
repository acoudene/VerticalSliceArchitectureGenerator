// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Feature.Api;

[ApiController]
[Route("api/[controller]")]
public class EntityNameController : ControllerBase
{
  /// <remarks>
  /// If tests are done with Swagger for example, in case of using inheritance, don't forget to manually add $type to json definition of DTO parameter
  /// Example: on a POST call, you should add ("$type" must be at the first line of json properties!!!)
  /// {
  ///   "$type": "entityName.entityNameInherited",
  ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  /// }
  /// </remarks>
  /// 

  private readonly ILogger<EntityNameController> _logger;

  protected RestComponent<EntityNameDto, EntityName, IEntityNameRepository> RestComponent { get => _restComponent; }
  private readonly RestComponent<EntityNameDto, EntityName, IEntityNameRepository> _restComponent;

  public EntityNameController(ILogger<EntityNameController> logger, IEntityNameRepository repository)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _restComponent = new RestComponent<EntityNameDto, EntityName, IEntityNameRepository>(repository);
  }

  // This commented part could be used to have benefits of json entity typing
  //protected virtual EntityNameDtoBase ToDto(EntityNameBase entity)
  //  => entity.ToInheritedDto();

  // This commented part could be used to have benefits of json entity typing
  //protected virtual EntityNameBase ToEntity(EntityNameDtoBase dto)
  //  => dto.ToInheritedEntity();

  protected virtual EntityNameDto ToDto(EntityName entity)
    => entity.ToDto();

  protected virtual EntityName ToEntity(EntityNameDto dto)
  => dto.ToEntity();

  [HttpGet]
  public virtual async Task<Results<Ok<List<EntityNameDto>>, BadRequest, ProblemHttpResult>> GetAllAsync()
  {
    try
    {
      return TypedResults.Ok(await _restComponent.GetAllAsync(ToDto));
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

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(Guid id)
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
  public virtual async Task<Results<Ok<List<EntityNameDto>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids)
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
  public virtual async Task<Results<Created<EntityNameDto>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] EntityNameDto newDto)
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
    [FromBody] EntityNameDto updatedDto)
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
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(Guid id)
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
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id,
    [FromBody] JsonPatchDocument<EntityNameDto> patchDto)
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
