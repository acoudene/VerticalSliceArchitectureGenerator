// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Feature.Localization;
using Feature.ViewObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Feature.RazorComponents;

public partial class EntityNamesTable
{
  private string _searchString = string.Empty;

  /// Use it if needed for row edition template: private EntityNameVoFluentValidator _entityNameValidator = new EntityNameVoFluentValidator();

  [Inject]
  public required IStringLocalizer<FeatureResource> Localizer { get; set; }

  [Parameter, EditorRequired]
  public required IEnumerable<EntityNameVo> ViewObjects { get; set; }

  [Parameter]
  public EventCallback<IEnumerable<EntityNameVo>?> ViewObjectsChanged { get; set; }

  [Parameter]
  public HashSet<EntityNameVo>? SelectedViewObjects { get; set; }

  [Parameter]
  public EntityNameVo? SelectedViewObject { get; set; }

  [Parameter]
  public EventCallback<HashSet<EntityNameVo>> OnSelectedItemsChanged { get; set; }

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

    if (ViewObjects is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObjects)}");
  }

  private bool FilterFunc(EntityNameVo vo)
  {
    return vo switch
    {
      EntityNameVo x when x.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      EntityNameVo x when x.CreatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      EntityNameVo x when x.UpdatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      // TODO - Complete with search filter in grid

      EntityNameVo x when x.Metadata is not null && x.Metadata.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      null => false,
      _ => false
    };
  }

  private ElementComparer EntityNameVoComparer = new();

  class ElementComparer : IEqualityComparer<EntityNameVo>
  {
    public bool Equals(EntityNameVo? a, EntityNameVo? b) => a?.Id == b?.Id;
    public int GetHashCode(EntityNameVo x) => HashCode.Combine(x?.Id);
  }
}
