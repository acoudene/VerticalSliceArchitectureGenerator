namespace UseCase.Api;

public static class EntityNameDtoEntityExtensions
{
  public static EntityNameDto ToDto(this EntityName entity)
  {
    return new EntityNameDto()
    {
      Id = entity.Id
    };
  }

  public static EntityName ToEntity(this EntityNameDto dto)
  {
    return new EntityName()
    {
      Id = dto.Id
    };
  }
}
