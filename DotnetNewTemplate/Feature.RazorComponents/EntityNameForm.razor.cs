using Microsoft.AspNetCore.Components;
using MudBlazor;
using Feature.ViewObjects;
using Feature.ViewObjects.Validators;

namespace Feature.RazorComponents;

public partial class EntityNameForm : ComponentBase
{
  [Parameter, EditorRequired]
  public EntityNameVo Vo { get; set; } = null!;

  [Parameter]
  public Func<Task>? DoSubmitAsync { get; set; }

  private MudForm _form = null!;

  private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  protected override Task OnInitializedAsync()
  {
    if (Vo is null)
      throw new InvalidOperationException(nameof(Vo));

    return Task.CompletedTask;
  }

  private async Task ValidateSubmit()
  {
    if (_form is null)
      throw new InvalidOperationException("Problem while referencing form");

    await _form.Validate();

    if (!_form.IsValid)
      return;

    if (DoSubmitAsync is null)
      return;

    await DoSubmitAsync();
  }
}
