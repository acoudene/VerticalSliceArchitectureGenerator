// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.Dtos;
using Feature.ViewObjects;

namespace Feature.Api.BackendForFrontend;

public static class EntityNameVoExtensions
{
  public static EntityNameVo ToViewObject(this EntityNameDto dto)
  {
    if (dto is null)
      return null!;

    switch (dto)
    {
      case EntityNameDto:
        {
          return new EntityNameVo()
          {
            Id = dto.Id,

            // TODO - EntityMapping - Dto to ViewObject to complete
          };
        }

      default:
        throw new NotImplementedException();
    }
  }

  public static EntityNameDto ToDto(this EntityNameVo viewObject)
  {
    if (viewObject is null)
      return null!;

    switch (viewObject)
    {
      case EntityNameVo:
        {
          return new EntityNameDto()
          {
            Id = viewObject.Id,

            // TODO - EntityMapping - ViewObject to Dto to complete
          };
        }

      default:
        throw new NotImplementedException();
    }
  }
}
