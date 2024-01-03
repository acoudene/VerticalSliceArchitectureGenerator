using System.Collections;

namespace Feature.Host.Tests;

internal class EntityNamesData : IEnumerable<object[]>
{
  public IEnumerator<object[]> GetEnumerator()
  {
    yield return new object[]
    {
      new List<EntityNameDto>()
      {
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
      }
    };
    yield return new object[]
    {
      new List<EntityNameDto>()
      {
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
      }
    };
    yield return new object[]
    {
      new List<EntityNameDto>()
      {
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
      }
    };
  }
  
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

