// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace UseCase.Dtos;

// This commented part could be used to have benefits of json entity typing
//[JsonDerivedType(typeof(EntityNameInheritedDto), EntityNameInheritedDto.TypeId)]
public class EntityNameDto : IIdentifierDto
{
  public Guid Id { get; set; }

  // TODO - EntityProperties - Fields to complete
}

// This commented part could be used to have benefits of json entity typing
// Example of inherited class
//public record EntityNameInheritedDto : EntityNameDtoBase
//{
//  public const string TypeId = "entityName.entityNameInherited";
//}