// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Data.Entities;

// This commented part could be used to have benefits of entity typing
//public abstract record EntityNameBase : IIdentifierEntity

public record EntityName : IIdentifierEntity
{
  public required Guid Id { get; set; }

  // TODO - EntityProperties - Fields to complete
}

// This commented part could be used to have benefits of entity typing
// Example of inherited class
//public record EntityNameInherited : EntityNameBase
//{
//}