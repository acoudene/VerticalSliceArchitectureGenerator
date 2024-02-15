// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Api;

[ApiController]
[Route("api/[controller]")]
public class EntityNameController : RestControllerBase<EntityNameDto, EntityName, IEntityNameRepository>
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
