using Microsoft.AspNetCore.Components;

namespace Core.RazorComponents;

public partial class ForEach<T> : ComponentBase
{
  [Parameter]
  public IEnumerable<T>? Items { get; set; }

  [Parameter]
  public RenderFragment<T>? ChildContent { get; set; }
}