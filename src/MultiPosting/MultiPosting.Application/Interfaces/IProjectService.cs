using MultiPosting.Application.Dto;

namespace MultiPosting.Application.Interfaces;

public interface IProjectService
{
    Task<GetUserResourcesByProjectIdResponse> GetUserResourcesByProjectIdAsync(Guid projectId);
    Task<Guid> CreateProjectAsync(CreateProjectRequest request);
    Task CreateUserResourcesToProjectAsync(CreateUserResourceRequest request);
    Task DeleteUserResourcesFromProjectAsync(DeleteUserResourceRequest request);
}