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
  private EntityNameForm _form = null!;

  [Inject]
  protected ISnackbar Snackbar { get; set; } = null!;

  [Inject]
  protected ILogger<EntityNamePage> Logger { get; set; } = null!;

  [Inject]
  protected IEntityNameViewModel ViewModel { get; set; } = null!;

  [Inject]
  protected NavigationManager Navigation { get; set; } = null!;

  [Inject]
  protected IStringLocalizer<FeatureResource> Localizer { get; set; } = null!;

  [Parameter]
  public string? Id { get; set; } = null;

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

    if (ViewModel is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel)}");

    ViewModel.SelectedItem = new EntityNameVo() { Id = Guid.NewGuid() };         
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
     
    if (_form is null)
      throw new InvalidOperationException($"Missing {nameof(_form)}");

    if (firstRender)
    {
      if (!string.IsNullOrWhiteSpace(Id) && Guid.TryParse(Id, out Guid guid))
      {
        ViewModel.SelectedItem = await ViewModel.GetByIdAsync(guid);
      }
    }

    if (ViewModel.SelectedItem is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel.SelectedItem)}");
  }

  private async Task ValidateSubmitAsync()
  {
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
