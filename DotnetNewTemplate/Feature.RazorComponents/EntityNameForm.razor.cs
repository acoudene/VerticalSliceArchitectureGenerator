using Feature.Localization;
using Feature.ViewObjects;
using Feature.ViewObjects.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Feature.RazorComponents;

public partial class EntityNameForm
{
  [Inject]
  public required IStringLocalizer<FeatureResource> Localizer { get; set; }

  [Parameter, EditorRequired]
  public required EntityNameVo ViewObject { get; set; }

  private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

    if (ViewObject is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObject)}");
  }
}
