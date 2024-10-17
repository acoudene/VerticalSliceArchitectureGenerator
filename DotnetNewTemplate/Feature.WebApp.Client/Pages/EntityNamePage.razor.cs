using Feature.Localization;
using Feature.RazorComponents;
using Feature.ViewModels;
using Feature.ViewObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Feature.WebApp.Client.Pages;

public partial class EntityNamePage : ComponentBase
{
  private EntityNameForm? _form;

  [Inject]
  public required ISnackbar Snackbar { get; set; }

  [Inject]
  public required ILogger<EntityNamePage> Logger { get; set; }

  [Inject]
  public required IEntityNameViewModel ViewModel { get; set; }

  [Inject]
  public required NavigationManager Navigation { get; set; }

  [Inject]
  public required IStringLocalizer<FeatureResource> Localizer { get; set; }

  [Parameter]
  public string? Id { get; set; } = null;

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Missing {nameof(Localizer)}");

    if (ViewModel is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel)}");

  }

  protected async Task InitByIdAsync()
  {
    ViewModel.SelectedItem = new EntityNameVo() { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow };

    if (!string.IsNullOrWhiteSpace(Id) && Guid.TryParse(Id, out Guid guid))
    {
      ViewModel.SelectedItem = await ViewModel.GetByIdAsync(guid);
    }

    if (ViewModel.SelectedItem is null)
    {
      throw new InvalidOperationException($"Missing {nameof(ViewModel.SelectedItem)}");
    }
  }

  private async Task ValidateSubmitAsync()
  {
    if (_form is null)
      return;

    if (ViewModel.SelectedItem is null)
      return;

    await _form.Validate();

    if (!_form.IsValid)
      return;

    await ViewModel.CreateOrUpdateAsync(ViewModel.SelectedItem);
    Snackbar.Add(Localizer["Saved!"]);
    Navigation.NavigateTo("/entityNames");
  }
}
