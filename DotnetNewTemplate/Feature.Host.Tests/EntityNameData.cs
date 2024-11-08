// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using System.Collections;

namespace Feature.Host.Tests;

internal class EntityNameData : IEnumerable<object[]>
{
  public IEnumerator<object[]> GetEnumerator()
  {
    yield return new object[] 
    {
      new EntityNameDto()
      {
        Id = Guid.NewGuid()
        // TODO - EntityProperties - Fields to complete
      }
    };
    yield return new object[]
    {
      new EntityNameDto()
      {
        Id = Guid.NewGuid()
        // TODO - EntityProperties - Fields to complete
      }
    };
    yield return new object[]
    {
      new EntityNameDto()
      {
        Id = Guid.NewGuid()
        // TODO - EntityProperties - Fields to complete
      }
    };
  }
  
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

