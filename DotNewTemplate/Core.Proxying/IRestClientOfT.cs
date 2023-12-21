using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace $safeprojectname$;

public interface IRestClient<TDto> where TDto : class, IIdentifierDto
{
  Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
  
  Task<TDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
  
  Task<HttpResponseMessage> CreateAsync(
    TDto dto,
    bool checkSuccessStatusCode = true, 
    CancellationToken cancellationToken = default);
  
  Task<HttpResponseMessage> UpdateAsync(
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