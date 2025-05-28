// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.Localization;
using Feature.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Feature.WebApp.Client.Pages;

public partial class EntityNamesPage : ComponentBase
{
  [Inject]
  public required ISnackbar Snackbar { private get; init; }

  [Inject]
  public required ILogger<EntityNamesPage> Logger { private get; init; } 

  [Inject]
  public required IEntityNameViewModel ViewModel { private get; init; } 

  [Inject]
  public required NavigationManager Navigation { private get; init; } 

  [Inject]
  public required IStringLocalizer<FeatureResource> Localizer { private get; init; } 

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
