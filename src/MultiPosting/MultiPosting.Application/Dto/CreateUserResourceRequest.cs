namespace MultiPosting.Application.Dto;

public class CreateUserResourceRequest
{
    public Guid ProjectId { get; init; }
    public List<UserResourceDto> UserResourcesDto { get; init; }
}