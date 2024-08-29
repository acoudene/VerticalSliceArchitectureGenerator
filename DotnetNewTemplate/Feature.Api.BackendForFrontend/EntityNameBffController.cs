// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.Proxies;
using Feature.ViewObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace Feature.Api.BackendForFrontend;

[ApiController]
[Route("api/[controller]")]
public class EntityNameBffController : ControllerBase
{
  private readonly ILogger<EntityNameBffController> _logger;
  private readonly IEntityNameClient _client;


  public EntityNameBffController(ILogger<EntityNameBffController> logger, IEntityNameClient client)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _client = client ?? throw new ArgumentNullException(nameof(client));
  }

  [HttpGet]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<List<EntityNameVo>>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      return (await _client.GetAllAsync(cancellationToken))
        .Select(dto => dto.ToViewObject())
        .ToList();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }

  [HttpPost("CreateOrUpdate")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<EntityNameVo>> CreateOrUpdateAsync(
     [FromBody] EntityNameVo newOrToUpdateVo,
     CancellationToken cancellationToken = default)
  {
    try
    {
      if (newOrToUpdateVo is null)
        throw new ArgumentNullException(nameof(newOrToUpdateVo));

      var dto = newOrToUpdateVo.ToDto();
      if (dto is null)
        throw new InvalidOperationException("Problem while converting to view object");

      await _client.CreateOrUpdateAsync(dto, cancellationToken);
      return dto.ToViewObject();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }
}
