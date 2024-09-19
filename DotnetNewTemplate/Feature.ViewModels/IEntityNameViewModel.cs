using Core.ViewModels;
using Feature.ViewObjects;

namespace Feature.ViewModels;

public interface IEntityNameViewModel : IViewModel<EntityNameVo>
{
  IEnumerable<EntityNameVo> Items { get; set; }
  HashSet<EntityNameVo> SelectedItems { get; set; }
  EntityNameVo? SelectedItem { get; set; }
}