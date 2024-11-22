// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels;
using Feature.ViewObjects;

namespace Feature.ViewModels;

/// <summary>
/// Dedicated interface to manage ViewModel for a dedicated entity
/// </summary>
public interface IEntityNameViewModel : IViewModel<EntityNameVo>
{
  IEnumerable<EntityNameVo> Items { get; set; }
  HashSet<EntityNameVo> SelectedItems { get; set; }
  EntityNameVo? SelectedItem { get; set; }
}