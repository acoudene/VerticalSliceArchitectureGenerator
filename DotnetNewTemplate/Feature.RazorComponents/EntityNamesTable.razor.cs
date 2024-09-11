using Feature.ViewObjects;
using Feature.ViewObjects.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Feature.RazorComponents;

public partial class EntityNamesTable
{
  private string _searchString = string.Empty;
  private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  [Parameter, EditorRequired]
  public IEnumerable<EntityNameVo> ViewObjects { get; set; } = null!;

  [Parameter]
  public HashSet<EntityNameVo>? SelectedViewObjects { get; set; }

  [Parameter]
  public EntityNameVo? SelectedViewObject { get; set; }

  [Parameter]
  public EventCallback<HashSet<EntityNameVo>> OnSelectedItemsChanged { get; set; }

  protected override Task OnInitializedAsync()
  {
    if (ViewObjects is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObjects)}");

    return Task.CompletedTask;
  }

  private bool FilterFunc(EntityNameVo vo)
  {
    return vo switch
    {
      EntityNameVo x when x.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      EntityNameVo x when x.CreatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      EntityNameVo x when x.UpdatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      // TODO - Complete with search filter in grid

      null => false,
      var other => false
    };
  }
}
