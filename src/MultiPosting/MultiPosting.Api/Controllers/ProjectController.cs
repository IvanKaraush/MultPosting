using Microsoft.AspNetCore.Mvc;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;

namespace MultiPosting.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetUserResources(Guid projectId)
    {
        var userResources = await _projectService.GetUserResourcesByProjectIdAsync(projectId);
        return Ok(userResources);
    }

    [HttpGet("user-resources/{projectId:guid}")]
    public async Task<IActionResult> GetIsSelectedUserResources(Guid projectId)
    {
        var userResources = await _projectService.GetIsSelectedUserResourcesByProjectIdAsync(projectId);
        return Ok(userResources);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var projectId = await _projectService.CreateProjectAsync(request);
        return Ok(projectId);
    }

    [HttpPost("user-resources")]
    public async Task<IActionResult> CreateUserResource([FromBody] CreateUserResourceRequest request)
    {
        await _projectService.CreateUserResourcesToProjectAsync(request);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserResource([FromQuery] DeleteUserResourceRequest request)
    {
        await _projectService.DeleteUserResourcesFromProjectAsync(request);
        return NoContent();
    }
}