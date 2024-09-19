using Feature.Localization;
using Feature.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Feature.WebApp.Client.Pages;

public partial class EntityNamesPage : ComponentBase
{
  [Inject]
  protected ISnackbar Snackbar { get; set; } = null!;

  [Inject]
  protected ILogger<EntityNamesPage> Logger { get; set; } = null!;

  [Inject]
  protected IEntityNameViewModel ViewModel { get; set; } = null!;

  [Inject]
  protected NavigationManager Navigation { get; set; } = null!;

  [Inject]
  protected IStringLocalizer<FeatureResource> Localizer { get; set; } = null!;

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Missing {nameof(Localizer)}");

    if (ViewModel is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel)}");
  }

  protected Task AddViewObjectAsync()
  {
    Navigation.NavigateTo("/entityName");
    return Task.CompletedTask;
  }

  protected Task UpdateViewObjectAsync()
  {
    if (ViewModel.SelectedItems.Count != 1)
      return Task.CompletedTask;

    Guid id = ViewModel.SelectedItems.Single().Id;
    if (id == Guid.Empty)
      return Task.CompletedTask;

    Navigation.NavigateTo($"/entityName/{id}");
    return Task.CompletedTask;
  }

  protected async Task RemoveViewObjectAsync()
  {
    foreach (var item in ViewModel.SelectedItems)
    {
      await ViewModel.RemoveAsync(item.Id);
    }

    Snackbar.Add(Localizer["Deleted!"]);
    ViewModel.Items = await ViewModel.GetAllAsync();
  }
}
