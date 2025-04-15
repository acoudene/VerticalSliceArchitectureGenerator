// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Host.Tests;

internal class EntityNamesData : TheoryData<List<EntityNameDto>>
{
  public EntityNamesData()
  {
    Add([
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);

    Add([
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);

    Add([
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new EntityNameDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);
  }
}

