namespace $safeprojectname$;

public static class $ext_entityName$DtoEntityExtensions
{
  public static $ext_entityName$Dto ToDto(this $ext_entityName$ entity)
  {
    return new $ext_entityName$Dto()
    {
      Id = entity.Id
    };
  }

  public static $ext_entityName$ ToEntity(this $ext_entityName$Dto dto)
  {
    return new $ext_entityName$()
    {
      Id = dto.Id
    };
  }
}
