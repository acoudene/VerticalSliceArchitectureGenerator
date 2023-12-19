namespace $safeprojectname$;

public static class RequestFormDtoEntityExtensions
{
  public static RequestFormDto ToDto(this RequestForm entity)
  {
    return new RequestFormDto(entity.FormPropertiesTemplate.ToDto())
    {
      Id = entity.Id,
      Fields = entity
      .Fields
      .Select(childEntity => childEntity.ToInheritedDto())
      .ToList()
    };
  }

  public static RequestForm ToEntity(this RequestFormDto dto)
  {
    return new RequestForm(dto.FormPropertiesTemplate.ToEntity())
    {
      Id = dto.Id,
      Fields = dto
      .Fields
      .Select(childDto => childDto.ToInheritedEntity())
      .ToList()
    };
  }
}
