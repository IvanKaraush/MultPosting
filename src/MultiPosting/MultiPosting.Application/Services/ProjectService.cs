using Mapster;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;
using MultiPosting.Infrastructure.Interfaces;
using MultPosting.Domain.Entities;
using Shared.Application.Primitives;
using Shared.Domain.Exceptions;

namespace MultiPosting.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<GetUserResourcesByProjectIdResponse> GetUserResourcesByProjectIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetFullById(projectId)
                      ?? throw new EntityNotFoundException(string.Format(ExceptionMessages.EntityNotFound,
                          projectId));

        return project.Adapt<GetUserResourcesByProjectIdResponse>();
    }

    public async Task<List<UserResourceDto>> GetIsSelectedUserResourcesByProjectIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetFullById(projectId)
                      ?? throw new EntityNotFoundException(string.Format(ExceptionMessages.EntityNotFound,
                          projectId));
        return project.UserResources.Where(c => c.IsSelected).ToList().Adapt<List<UserResourceDto>>();

    }

    public async Task<Guid> CreateProjectAsync(CreateProjectRequest request)
    {
        var project = new Project(Guid.NewGuid(), request.Name);
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();
        return project.Id;
    }

    public async Task CreateUserResourcesToProjectAsync(CreateUserResourceRequest request)
    {
        var project = await _projectRepository.GetFullById(request.ProjectId)
                      ?? throw new EntityNotFoundException(string.Format(ExceptionMessages.EntityNotFound,
                          request.ProjectId));
        request.UserResourcesDto.ForEach(c =>
            project.AddUserResourceIfNotExist(Guid.NewGuid(), c.Name, c.ImageUrl, c.IsSelected, c.SocialMedia));
        await _projectRepository.SaveChangesAsync();
    }

    public async Task DeleteUserResourcesFromProjectAsync(DeleteUserResourceRequest request)
    {
        var project = await _projectRepository.GetFullById(request.ProjectId)
                      ?? throw new EntityNotFoundException(string.Format(ExceptionMessages.EntityNotFound,
                          request.ProjectId));

        project.DeleteUserResource(request.UserResourceId);
        await _projectRepository.SaveChangesAsync();
    }
}