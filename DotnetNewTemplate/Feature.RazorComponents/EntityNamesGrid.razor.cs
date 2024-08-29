using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Feature.ViewObjects;

namespace Feature.RazorComponents;

public partial class EntityNamesGrid : ComponentBase
{
  private int _progressIncrement = 0;

  [Inject]
  protected ISnackbar Snackbar { get; set; } = null!;

  [Inject]
  protected ILogger<EntityNamesGrid> Logger { get; set; } = null!;

  [Parameter, EditorRequired]
  public List<EntityNameVo> Vos { get; set; } = null!;
  private HashSet<EntityNameVo>? _selectedVos;

  private string _searchString = string.Empty;
  private string _accountLoginToAdd = string.Empty;

  protected override Task OnInitializedAsync()
  {
    if (Vos is null)
      throw new InvalidOperationException(nameof(Vos));

    return Task.CompletedTask;
  }

  private bool FilterFunc(EntityNameVo vo)
  {
    return vo switch
    {
      EntityNameVo x when x.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      // TODO - Complete with search filter in grid

      null => false,
      var other => false
    };
  }
}
