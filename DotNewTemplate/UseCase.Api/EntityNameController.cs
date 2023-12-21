namespace $safeprojectname$;

[ApiController]
[Route("api/[controller]")]
public class $ext_entityName$Controller : RestControllerBase<$ext_entityName$Dto, $ext_entityName$, I$ext_entityName$Repository>
{
  public $ext_entityName$Controller(I$ext_entityName$Repository repository)
    : base(repository)
  {

  }

  protected override $ext_entityName$Dto ToDto($ext_entityName$ entity)
    => entity.ToDto();

  protected override $ext_entityName$ ToEntity($ext_entityName$Dto dto)
    => dto.ToEntity();
}
