using Core.Api;
using Microsoft.AspNetCore.Mvc;

namespace $safeprojectname$;

[ApiController]
[Route("api/[controller]")]
public class RequestFormController : RestControllerBase<RequestFormDto, RequestForm, IRequestFormRepository>
{
  public RequestFormController(IRequestFormRepository repository)
    : base(repository)
  {

  }

  protected override RequestFormDto ToDto(RequestForm entity)
    => entity.ToDto();

  protected override RequestForm ToEntity(RequestFormDto dto)
    => dto.ToEntity();
}
