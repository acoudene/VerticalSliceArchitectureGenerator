// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.Proxying;

public interface IRestClient<TDto> where TDto : class, IIdentifierDto
{
  Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
  
  Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task<List<TDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

  Task<HttpResponseMessage> CreateAsync(
    TDto dto,
    bool checkSuccessStatusCode = true, 
    CancellationToken cancellationToken = default);
  
  Task<HttpResponseMessage> UpdateAsync(
    Guid id,
    TDto dto,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken= default);
  
  Task<TDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
  
  Task<HttpResponseMessage> PatchAsync(
    Guid id, 
    JsonPatchDocument<TDto> patch,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default);
}