using MultiPosting.Application.Dto;

namespace MultiPosting.Application.Interfaces;

public interface IProjectService
{
    Task<GetUserResourcesByProjectIdResponse> GetUserResourcesByProjectId(Guid projectId);
    Task<Guid> CreateProjectAsync(CreateProjectRequest request);
    Task CreateUserResourcesToProject(CreateUserResourceRequest request);
}