using Feature.Localization;
using Feature.ViewObjects;
using Feature.ViewObjects.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Feature.RazorComponents;

public partial class EntityNameForm
{
	[Inject]
	protected IStringLocalizer<FeatureResource> Localizer { get; set; } = null!;

	[Parameter, EditorRequired]
  public EntityNameVo ViewObject { get; set; } = null!;

  private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  protected override Task OnInitializedAsync()
  {		
		if (Localizer is null)
			throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

		if (ViewObject is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObject)}");

    return Task.CompletedTask;
  }
}
