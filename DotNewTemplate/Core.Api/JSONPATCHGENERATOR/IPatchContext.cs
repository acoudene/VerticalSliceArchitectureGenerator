using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;

namespace $safeprojectname$.JsonPatchGenerator;

public interface IPatchContext
{
  JsonPatchDocument Document { get; }

  JsonPatchOptions Options { get; }

  void CreatePatch(JToken original, JToken modified, JsonPatchPath path);
}