// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Presentation;

public static class EntityNameVoDtoExtensions
{
  public static EntityNameDto ToDto(this EntityNameVo entity)
  {
    return new EntityNameDto()
    {
      Id = entity.Id

      // TODO - EntityMapping - View Object to Dto to complete
    };
  }

  public static EntityNameVo ToViewObject(this EntityNameDto dto)
  {
    return new EntityNameVo()
    {
      Id = dto.Id

      // TODO - EntityMapping - Dto to View Object to complete
    };
  }
}
