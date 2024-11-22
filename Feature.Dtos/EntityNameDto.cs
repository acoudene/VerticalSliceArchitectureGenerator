// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Dtos;

// This commented part could be used to have benefits of json entity typing
//[JsonPolymorphic]
//[JsonDerivedType(typeof(EntityNameInheritedDto), EntityNameInheritedDto.TypeId)]
public record EntityNameDto : IIdentifierDto, ITimestampedDto
{
  public required Guid Id { get; set; }

  public DateTimeOffset CreatedAt { get; init; }

  public DateTimeOffset UpdatedAt { get; init; }

  // TODO - EntityProperties - Fields to complete

  public string? Metadata { get; set; } // Example, to remove if needed
}

// This commented part could be used to have benefits of json entity typing
// Example of inherited class
//[JsonDerivedType(typeof(EntityNameInheritedDto), EntityNameInheritedDto.TypeId)]
//public record EntityNameInheritedDto : EntityNameDtoBase
//{
//  public const string TypeId = "entityName.entityNameInherited";
//}