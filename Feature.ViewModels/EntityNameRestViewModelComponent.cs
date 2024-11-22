// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels;
using Feature.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels;

public class EntityNameRestViewModelComponent : RestViewModelComponent<EntityNameVo, IEntityNameRestBffClient>
{
  public EntityNameRestViewModelComponent(IEntityNameRestBffClient client) : base(client)
  {
  }
}
