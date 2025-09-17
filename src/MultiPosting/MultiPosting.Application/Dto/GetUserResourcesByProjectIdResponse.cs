namespace MultiPosting.Application.Dto;

public class GetUserResourcesByProjectIdResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool IsSelected { get; init; }
    public List<UserResourceDto> UserResources { get; init; }
}