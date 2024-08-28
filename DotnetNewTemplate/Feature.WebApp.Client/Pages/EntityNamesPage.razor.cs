using Microsoft.AspNetCore.Components;
using MudBlazor;
using Feature.ViewModels;
using Feature.ViewObjects;

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

  private List<EntityNameVo> _vos = Enumerable.Empty<EntityNameVo>().ToList();

  protected override async Task OnInitializedAsync()
  {
    if (ViewModel is null)
      throw new InvalidOperationException(nameof(ViewModel));

    _vos = await ViewModel.GetAllAsync();
  }

  protected Task AddViewObjectAsync()
  {
    Navigation.NavigateTo("/entityName");
    return Task.CompletedTask;
  }

  protected async Task UpdateViewObjectAsync()
  {
    await Task.CompletedTask;
  }

  protected async Task RemoveViewObjectAsync()
  {
    await Task.CompletedTask;
  }
}
