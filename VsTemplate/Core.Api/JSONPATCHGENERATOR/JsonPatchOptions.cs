using $safeprojectname$.JsonPatchGenerator.Handlers;

namespace $safeprojectname$.JsonPatchGenerator;

public class JsonPatchOptions
{
  public List<IPatchTypeHandler> Handlers { get; set; } = new()
          {new ArrayPatchTypeHandler(), new ObjectPatchTypeHandler(), new ReplaceValuePatchTypeHandler()};
}