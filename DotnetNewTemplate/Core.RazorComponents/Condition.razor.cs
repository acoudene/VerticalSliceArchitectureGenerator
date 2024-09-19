using Microsoft.AspNetCore.Components;

namespace Core.RazorComponents;

public partial class Condition : ComponentBase
{
  [Parameter]
  public bool Predicate { get; set; }

  [Parameter]
  public RenderFragment? ChildContent { get => Then; set => Then=value; }

  [Parameter]
  public RenderFragment? Then { get; set; }

  [Parameter]
  public RenderFragment? Else { get; set; }
}
