using Feature.ViewModels;
using Feature.ViewObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Feature.WebApp.Client.Pages;

public partial class EntityNamePage : ComponentBase
{
  [Inject]
  protected ISnackbar Snackbar { get; set; } = null!;

  [Inject]
  protected ILogger<EntityNamePage> Logger { get; set; } = null!;

  [Inject]
  protected IEntityNameViewModel ViewModel { get; set; } = null!;

  [Inject]
  protected NavigationManager Navigation { get; set; } = null!;

  [Parameter]
  public Guid? Id { get; set; } = null;

  public EntityNameVo Vo { get; set; } = new EntityNameVo() { Id = Guid.NewGuid() };

  protected override async Task OnInitializedAsync()
  {
    if (ViewModel is null)
      throw new InvalidOperationException(nameof(ViewModel));

    if (Id is not null)
    {
      Vo = await ViewModel.GetByIdAsync(Id.Value) ?? Vo;
    }

    if (Vo is null)
      throw new InvalidOperationException(nameof(Vo));
  }

  private async Task DoSubmitAsync()
  {
    await ViewModel.CreateOrUpdateAsync(Vo);
    Snackbar.Add("Submitted!");
    Navigation.NavigateTo("/entityNames");
  }
}
