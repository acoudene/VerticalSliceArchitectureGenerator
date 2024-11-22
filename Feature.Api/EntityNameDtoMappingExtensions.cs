// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Feature.Api;

public static class EntityNameDtoMappingExtensions
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
      Id = entity.Id,
      CreatedAt = entity.CreatedAt,
      UpdatedAt = entity.UpdatedAt,

      // TODO - EntityMapping - Business Entity to Dto to complete

      Metadata = entity.Metadata,
    };
  }

  public static EntityName ToEntity(this EntityNameDto dto)
  {
    return new EntityName()
    {
      Id = dto.Id,
      CreatedAt = dto.CreatedAt,
      UpdatedAt = dto.UpdatedAt,

      // TODO - EntityMapping - Dto to Business Entity to complete

      Metadata = dto.Metadata,
    };
  }
}
