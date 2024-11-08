// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Feature.ViewObjects;

namespace Feature.ViewModels.BffProxying;

/// <summary>
/// Interface to manage client/server interaction as a REST proxy
/// </summary>
public interface IEntityNameRestBffClient : IRestBffClient<EntityNameVo>
{
}
