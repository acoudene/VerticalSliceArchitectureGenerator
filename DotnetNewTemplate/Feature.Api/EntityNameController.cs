// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Api;

[ApiController]
[Route("api/[controller]")]
public class EntityNameController : RestControllerBase<EntityNameDto, EntityName, IEntityNameRepository>
{
  public EntityNameController(IEntityNameRepository repository)
    : base(repository)
  {

  }

  // This commented part could be used to have benefits of json entity typing
  //protected override EntityNameDtoBase ToDto(EntityNameBase entity)
  //  => entity.ToInheritedDto();

  // This commented part could be used to have benefits of json entity typing
  //protected override EntityNameBase ToEntity(EntityNameDtoBase dto)
  //  => dto.ToInheritedEntity();

  protected override EntityNameDto ToDto(EntityName entity)
    => entity.ToDto();

  protected override EntityName ToEntity(EntityNameDto dto)
    => dto.ToEntity();
}
