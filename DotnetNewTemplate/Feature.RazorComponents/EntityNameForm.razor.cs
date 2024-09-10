using Feature.ViewObjects;
using Feature.ViewObjects.Validators;
using Microsoft.AspNetCore.Components;

namespace Feature.RazorComponents;

public partial class EntityNameForm
{
  [Parameter, EditorRequired]
  public EntityNameVo ViewObject { get; set; } = null!;

  private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  protected override Task OnInitializedAsync()
  {
    if (ViewObject is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObject)}");

    return Task.CompletedTask;
  }
}
