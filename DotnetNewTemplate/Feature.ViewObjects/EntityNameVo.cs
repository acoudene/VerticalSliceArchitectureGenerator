// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewObjects;

namespace Feature.ViewObjects;

// This commented part could be used to have benefits of json entity typing
//[JsonPolymorphic]
//[JsonDerivedType(typeof(EntityInheritedVo), EntityInheritedVo.TypeId)]
public record EntityNameVo : IIdentifierViewObject, ITimestampedViewObject
{
  public required Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }

  // TODO - EntityProperties - Fields to complete

}

// This commented part could be used to have benefits of json entity typing
// Example of inherited class
//[JsonDerivedType(typeof(EntityInheritedVo), EntityInheritedVo.TypeId)]
//public record EntityInheritedVo : EntityVoBase
//{
//  public const string TypeId = "article.articleInherited";
//}