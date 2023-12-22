namespace UseCase.Api;

[ApiController]
[Route("api/[controller]")]
public class EntityNameController : RestControllerBase<EntityNameDto, EntityName, IEntityNameRepository>
{
  public EntityNameController(IEntityNameRepository repository)
    : base(repository)
  {

  }

  protected override EntityNameDto ToDto(EntityName entity)
    => entity.ToDto();

  protected override EntityName ToEntity(EntityNameDto dto)
    => dto.ToEntity();
}
