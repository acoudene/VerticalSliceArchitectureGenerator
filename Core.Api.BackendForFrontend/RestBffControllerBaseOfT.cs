// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Core.Proxying;
using Core.ViewObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Api.BackendForFrontend;

/// <summary>
/// Rest base API to manage backend for frontend aspect
/// </summary>
/// <typeparam name="TViewObject"></typeparam>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TClient"></typeparam>
public abstract class RestBffControllerBase<TViewObject, TDto, TClient> : ControllerBase
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
  where TClient : IRestClient<TDto>
{
  private readonly ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> _logger;
  private readonly RestBffBehavior<TViewObject, TDto, TClient> _behavior;

  protected ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> Logger => _logger;
  protected RestBffBehavior<TViewObject, TDto, TClient> Behavior => _behavior;

  protected RestBffControllerBase(
    ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> logger,
    RestBffBehavior<TViewObject, TDto, TClient> behavior)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
  }

  protected RestBffControllerBase(
    ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> logger, 
    TClient client)
      : this(logger, new RestBffBehavior<TViewObject, TDto, TClient>(client))
  { }

  protected abstract TViewObject ToViewObject(TDto dto);
  protected abstract TDto ToDto(TViewObject viewObject);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TViewObject>>, BadRequest, ProblemHttpResult>> GetAllAsync(
    CancellationToken cancellationToken = default)
    => TypedResults.Ok(await _behavior.GetAllAsync(ToViewObject, cancellationToken));

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var foundVo = await _behavior.GetByIdAsync(id, ToViewObject, cancellationToken);
    if (foundVo is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(foundVo);
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TViewObject>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    return TypedResults.Ok(await _behavior.GetByIdsAsync(ids, ToViewObject, cancellationToken));
  }

  [HttpPost]
  public virtual async Task<Results<Created<TViewObject>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] TViewObject newVo,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    return TypedResults.Created("{newVo.Id}", await _behavior.CreateAsync(newVo, ToDto, cancellationToken));
  }

  [HttpPost("CreateOrUpdate")]
  public virtual async Task<Results<NoContent, Created<TViewObject>, BadRequest, ProblemHttpResult>> CreateOrUpdateAsync(
      [FromBody] TViewObject newOrToUpdateVo,
      CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    if (newOrToUpdateVo is null)
      throw new ArgumentNullException(nameof(newOrToUpdateVo));

    Guid id = newOrToUpdateVo.Id;
    if (id == Guid.Empty)
      throw new ArgumentNullException(nameof(newOrToUpdateVo.Id));

    var updatedVo = await _behavior.UpdateAsync(id, newOrToUpdateVo, ToDto);
    if (updatedVo is not null)
      return TypedResults.NoContent();

    return TypedResults.Created("{newOrToUpdateVo.Id}", await _behavior.CreateAsync(newOrToUpdateVo, ToDto, cancellationToken));
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, BadRequest, ProblemHttpResult>> UpdateAsync(
    Guid id,
    [FromBody] TViewObject toUpdateVo,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var updatedVo = await _behavior.UpdateAsync(id, toUpdateVo, ToDto, cancellationToken);
    if (updatedVo is null)
      return TypedResults.NotFound();

    return TypedResults.NoContent();
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var deletedVo = await _behavior.DeleteAsync(id, ToViewObject, cancellationToken);
    if (deletedVo is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(deletedVo);
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id,
    [FromBody] JsonPatchDocument<TViewObject> toPatchVo,
    CancellationToken cancellationToken = default)
  {
    if (!ModelState.IsValid)
      throw new ArgumentException("ModelState is not validated or invalid");

    var patchedVo = await _behavior.PatchAsync(id, toPatchVo, ModelState, ToDto, ToViewObject, cancellationToken);
    if (patchedVo is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(patchedVo);
  }
}
