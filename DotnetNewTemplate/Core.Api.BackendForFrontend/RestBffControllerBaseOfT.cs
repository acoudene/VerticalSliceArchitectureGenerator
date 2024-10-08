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

public abstract class RestBffControllerBase<TViewObject, TDto, TClient> : ControllerBase
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
  where TClient : IRestClient<TDto>
{
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> _logger;
  private readonly RestBffComponent<TViewObject, TDto, TClient> _restBffComponent;

  protected IHostEnvironment HostEnvironment => _hostEnvironment;
  protected ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> Logger => _logger;
  protected RestBffComponent<TViewObject, TDto, TClient> RestComponent => _restBffComponent;

  protected RestBffControllerBase(
    IHostEnvironment hostEnvironment,
    ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> logger,
    RestBffComponent<TViewObject, TDto, TClient> restBffComponent)
  {
    _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _restBffComponent = restBffComponent ?? throw new ArgumentNullException(nameof(restBffComponent));
  }

  protected RestBffControllerBase(IHostEnvironment hostEnvironment, ILogger<RestBffControllerBase<TViewObject, TDto, TClient>> logger, TClient client)
      : this(hostEnvironment, logger, new RestBffComponent<TViewObject, TDto, TClient>(client))
  { }

  protected abstract TViewObject ToViewObject(TDto dto);
  protected abstract TDto ToDto(TViewObject viewObject);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TViewObject>>, BadRequest, ProblemHttpResult>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      return TypedResults.Ok(await _restBffComponent.GetAllAsync(ToViewObject, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    try
    {
      var foundVo = await _restBffComponent.GetByIdAsync(id, ToViewObject, cancellationToken);
      if (foundVo is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(foundVo);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TViewObject>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids,
    CancellationToken cancellationToken = default)
  {
    try
    {
      return TypedResults.Ok(await _restBffComponent.GetByIdsAsync(ids, ToViewObject, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPost]
  public virtual async Task<Results<Created<TViewObject>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] TViewObject newVo,
    CancellationToken cancellationToken = default)
  {
    try
    {
      return TypedResults.Created("{newVo.Id}", await _restBffComponent.CreateAsync(newVo, ToDto, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPost("CreateOrUpdate")]
  public virtual async Task<Results<NoContent, Created<TViewObject>, BadRequest, ProblemHttpResult>> CreateOrUpdateAsync(
      [FromBody] TViewObject newOrToUpdateVo,
      CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({ViewObject})...", nameof(CreateOrUpdateAsync), newOrToUpdateVo);
      if (newOrToUpdateVo is null)
        throw new ArgumentNullException(nameof(newOrToUpdateVo));

      Guid id = newOrToUpdateVo.Id;
      if (id == Guid.Empty)
        throw new ArgumentNullException(nameof(newOrToUpdateVo.Id));

      var updatedVo = await _restBffComponent.UpdateAsync(id, newOrToUpdateVo, ToDto);
      if (updatedVo is not null)
        return TypedResults.NoContent();

      return TypedResults.Created("{newOrToUpdateVo.Id}", await _restBffComponent.CreateAsync(newOrToUpdateVo, ToDto, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
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
    [FromBody] TViewObject toUpdateVo,
    CancellationToken cancellationToken = default)
  {
    try
    {
      var updatedVo = await _restBffComponent.UpdateAsync(id, toUpdateVo, ToDto, cancellationToken);
      if (updatedVo is null)
        return TypedResults.NotFound();

      return TypedResults.NoContent();
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    try
    {
      var deletedVo = await _restBffComponent.DeleteAsync(id, ToViewObject, cancellationToken);
      if (deletedVo is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(deletedVo);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TViewObject>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id,
    [FromBody] JsonPatchDocument<TViewObject> toPatchVo,
    CancellationToken cancellationToken = default)
  {
    try
    {
      var patchedVo = await _restBffComponent.PatchAsync(id, toPatchVo, ModelState, ToDto, ToViewObject, cancellationToken);
      if (patchedVo is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(patchedVo);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }
}
