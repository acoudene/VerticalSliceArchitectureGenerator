// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace UseCase.Api;

public static class EntityNameDtoEntityExtensions
{
  // This commented part could be used to have benefits of json entity typing
  //public static EntityNameDtoBase ToInheritedDto(this EntityNameBase entity)
  //{
  //  switch (entity)
  //  {
  //    case EntityNameInherited inheritedEntity: return inheritedEntity.ToDto();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  // This commented part could be used to have benefits of json entity typing
  //public static EntityNameBase ToInheritedEntity(this EntityNameDtoBase dto)
  //{
  //  switch (dto)
  //  {
  //    case EntityNameInheritedDto inheritedDto: return inheritedDto.ToEntity();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  public static EntityNameDto ToDto(this EntityName entity)
  {
    return new EntityNameDto()
    {
      Id = entity.Id

      // TODO - EntityMapping - Business Entity to Dto to complete
    };
  }

  public static EntityName ToEntity(this EntityNameDto dto)
  {
    return new EntityName()
    {
      Id = dto.Id

      // TODO - EntityMapping - Dto to Business Entity to complete
    };
  }
}
