// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Feature.Api;

/// <summary>
/// Backend API interacting with proxy through DTOs
/// </summary>
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

  protected RestApiBehavior<EntityNameDto, EntityName, IEntityNameRepository> Behavior { get => _behavior; }
  private readonly RestApiBehavior<EntityNameDto, EntityName, IEntityNameRepository> _behavior;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="hostEnvironment"></param>
  /// <param name="logger"></param>
  /// <param name="repository"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public EntityNameController(ILogger<EntityNameController> logger, IEntityNameRepository repository)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = new RestApiBehavior<EntityNameDto, EntityName, IEntityNameRepository>(repository);
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

  /// <summary>
  /// Retrieves all items as a list of DTOs.
  /// </summary>
  /// <remarks>This method is asynchronous and returns a typed result that encapsulates the outcome of the
  /// operation.</remarks>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
  /// <returns>A result containing one of the following: <list type="bullet"> <item><description><see langword="Ok"/> with a list
  /// of DTOs if the operation succeeds.</description></item> <item><description><see langword="BadRequest"/> if the
  /// request is invalid.</description></item> <item><description><see langword="ProblemHttpResult"/> if an error occurs
  /// during processing.</description></item> </list></returns>
  [HttpGet]
  public virtual async Task<Results<Ok<List<EntityNameDto>>, BadRequest, ProblemHttpResult>> GetAllAsync(
    CancellationToken cancellationToken = default)
    => TypedResults.Ok(await _behavior.GetAllAsync(ToDto, cancellationToken));

  /// <summary>
  /// Retrieves an entity by its unique identifier.
  /// </summary>
  /// <remarks>This method uses the provided <paramref name="id"/> to locate the entity. If the entity is found,
  /// it is returned as a DTO. If the entity is not found, a 404 Not Found result is returned. Ensure that the <paramref
  /// name="id"/> is a valid GUID.</remarks>
  /// <param name="id">The unique identifier of the entity to retrieve.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
  /// <returns>A <see cref="Results{T1, T2, T3, T4}"/> object that represents one of the following outcomes: <list type="bullet">
  /// <item><description><see cref="TypedResults.Ok{T}"/> containing the entity if it is found.</description></item>
  /// <item><description><see cref="TypedResults.NotFound"/> if the entity does not exist.</description></item>
  /// <item><description><see cref="TypedResults.BadRequest"/> if the request is invalid.</description></item>
  /// <item><description><see cref="TypedResults.Problem"/> if an unexpected error occurs.</description></item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the <see cref="ModelState"/> is invalid.</exception>
  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var foundEntity = await _behavior.GetByIdAsync(id, ToDto, cancellationToken);
    if (foundEntity is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(foundEntity);
  }

  /// <summary>
  /// Retrieves a list of DTOs corresponding to the specified IDs.
  /// </summary>
  /// <remarks>This method validates the model state before processing the request. Ensure that the <paramref
  /// name="ids"/> parameter contains valid GUIDs and is not null or empty to avoid a <see cref="BadRequest"/>
  /// response.</remarks>
  /// <param name="ids">A list of unique identifiers representing the entities to retrieve.  The list must not be null or empty.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
  /// <returns>A result containing one of the following: <list type="bullet"> <item><description><see cref="Ok{T}"/> with a list
  /// of DTOs if the operation is successful.</description></item> <item><description><see cref="BadRequest"/> if the
  /// request is invalid.</description></item> <item><description><see cref="ProblemHttpResult"/> if an error occurs
  /// during processing.</description></item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the model state is invalid.</exception>
  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<EntityNameDto>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids,
    CancellationToken cancellationToken = default)
  {

    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    return TypedResults.Ok(await _behavior.GetByIdsAsync(ids, ToDto, cancellationToken));
  }

  /// <summary>
  /// Creates a new resource based on the provided data transfer object (DTO).
  /// </summary>
  /// <remarks>The created resource's location is included in the response. Ensure that the <paramref
  /// name="newDto"/> parameter is properly validated before calling this method.</remarks>
  /// <param name="newDto">The data transfer object representing the resource to be created. Cannot be null.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
  /// <returns>A result containing one of the following: <list type="bullet"> <item><description><see cref="Results{T1, T2,
  /// T3}"/> with a <see cref="Created{T}"/> result if the resource is successfully created.</description></item>
  /// <item><description><see cref="BadRequest"/> if the request is invalid.</description></item>
  /// <item><description><see cref="ProblemHttpResult"/> if an error occurs during processing.</description></item>
  /// </list></returns>
  /// <exception cref="ArgumentException">Thrown if the model state is invalid.</exception>
  [HttpPost]
  public virtual async Task<Results<Created<EntityNameDto>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] EntityNameDto newDto,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    return TypedResults.Created("{newDto.Id}", await _behavior.CreateAsync(newDto, ToEntity, cancellationToken));
  }

  /// <summary>
  /// Creates a new resource or updates an existing one based on the provided data transfer object (DTO).
  /// </summary>
  /// <remarks>This method determines whether to create or update a resource based on the <see cref="TDto.Id"/>
  /// property. If the ID is a valid GUID, the method attempts to update the resource; otherwise, it creates a new
  /// one.</remarks>
  /// <param name="newOrToUpdateDto">The data transfer object containing the details of the resource to create or update.  The <see cref="TDto.Id"/>
  /// property must be a valid GUID for updates; otherwise, a new resource will be created.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
  /// <returns>A result indicating the outcome of the operation: <list type="bullet"> <item><description><see cref="NoContent"/>
  /// if the resource was successfully updated.</description></item> <item><description><see cref="Created{TDto}"/> if a
  /// new resource was successfully created.</description></item> <item><description><see cref="BadRequest"/> if the
  /// input data is invalid.</description></item> <item><description><see cref="ProblemHttpResult"/> if an unexpected
  /// error occurs.</description></item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the model state is invalid.</exception>
  /// <exception cref="ArgumentNullException">Thrown if <paramref name="newOrToUpdateDto"/> is <see langword="null"/> or if <see cref="TDto.Id"/> is an empty
  /// GUID.</exception>
  [HttpPost("CreateOrUpdate")]
  public virtual async Task<Results<NoContent, Created<EntityNameDto>, BadRequest, ProblemHttpResult>> CreateOrUpdateAsync(
      [FromBody] EntityNameDto newOrToUpdateDto,
      CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    if (newOrToUpdateDto is null)
      throw new ArgumentNullException(nameof(newOrToUpdateDto));

    Guid id = newOrToUpdateDto.Id;
    if (id == Guid.Empty)
      throw new ArgumentNullException(nameof(newOrToUpdateDto.Id));

    var updatedDto = await _behavior.UpdateAsync(id, newOrToUpdateDto, ToEntity, cancellationToken);
    if (updatedDto is not null)
      return TypedResults.NoContent();

    return TypedResults.Created("{newOrToUpdateDto.Id}", await _behavior.CreateAsync(newOrToUpdateDto, ToEntity, cancellationToken));
  }

  /// <summary>
  /// Updates an existing entity with the specified identifier using the provided data transfer object (DTO).
  /// </summary>
  /// <remarks>The method validates the provided <paramref name="updatedDto"/> against the model state. If the
  /// model state is invalid, an <see cref="ArgumentException"/> is thrown.</remarks>
  /// <param name="id">The unique identifier of the entity to update.</param>
  /// <param name="updatedDto">The data transfer object containing the updated values for the entity.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
  /// <returns>A <see cref="Results{T1, T2, T3, T4}"/> object that represents one of the following outcomes: <list type="bullet">
  /// <item><description><see langword="NoContent"/> if the update is successful.</description></item>
  /// <item><description><see langword="NotFound"/> if no entity with the specified identifier
  /// exists.</description></item> <item><description><see langword="BadRequest"/> if the request is
  /// invalid.</description></item> <item><description><see langword="ProblemHttpResult"/> if an unexpected error
  /// occurs.</description></item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the <see cref="ModelState"/> is invalid.</exception>
  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, BadRequest, ProblemHttpResult>> UpdateAsync(
    Guid id,
    [FromBody] EntityNameDto updatedDto,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var updatedEntity = await _behavior.UpdateAsync(id, updatedDto, ToEntity, cancellationToken);
    if (updatedEntity is null)
      return TypedResults.NotFound();

    return TypedResults.NoContent();
  }

  /// <summary>
  /// Deletes an entity identified by the specified <paramref name="id"/>.
  /// </summary>
  /// <remarks>This method performs a deletion operation for an entity identified by its unique identifier.
  /// Ensure that the <paramref name="id"/> is a valid GUID and that the model state is valid before calling this
  /// method.</remarks>
  /// <param name="id">The unique identifier of the entity to delete.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
  /// <returns>A <see cref="Results{T1, T2, T3, T4}"/> object that represents the result of the operation: <list type="bullet">
  /// <item><description><see cref="TypedResults.Ok{T}"/> if the entity was successfully deleted.</description></item>
  /// <item><description><see cref="TypedResults.NotFound"/> if no entity with the specified <paramref name="id"/>
  /// exists.</description></item> <item><description><see cref="TypedResults.BadRequest"/> if the request is
  /// invalid.</description></item> <item><description><see cref="TypedResults.Problem"/> if an unexpected error
  /// occurs.</description></item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the model state is invalid.</exception>
  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var deletedEntity = await _behavior.DeleteAsync(id, ToDto, cancellationToken);
    if (deletedEntity is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(deletedEntity);
  }

  /// <summary>
  /// Applies a JSON Patch operation to an entity identified by its unique identifier.
  /// </summary>
  /// <remarks>This method uses the JSON Patch standard to apply partial updates to an entity. Ensure that the
  /// <paramref name="patchDto"/> is properly constructed and matches the structure of the target entity.</remarks>
  /// <param name="id">The unique identifier of the entity to be patched.</param>
  /// <param name="patchDto">A <see cref="JsonPatchDocument{T}"/> containing the set of operations to apply to the entity.</param>
  /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
  /// <returns>A <see cref="Results{T1, T2, T3, T4}"/> containing one of the following results: <list type="bullet"> <item>
  /// <description><see cref="Ok{T}"/> if the patch operation is successful, containing the updated
  /// entity.</description> </item> <item> <description><see cref="NotFound"/> if the entity with the specified
  /// <paramref name="id"/> does not exist.</description> </item> <item> <description><see cref="BadRequest"/> if the
  /// provided patch document is invalid or the model state is invalid.</description> </item> <item> <description><see
  /// cref="ProblemHttpResult"/> if an unexpected error occurs during the operation.</description> </item> </list></returns>
  /// <exception cref="ArgumentException">Thrown if the model state is invalid and cannot be processed.</exception>
  [HttpPatch]
  public virtual async Task<Results<Ok<EntityNameDto>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id,
    [FromBody] JsonPatchDocument<EntityNameDto> patchDto,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var patchedEntity = await _behavior.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto, cancellationToken);
    if (patchedEntity is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(patchedEntity);
  }
}
