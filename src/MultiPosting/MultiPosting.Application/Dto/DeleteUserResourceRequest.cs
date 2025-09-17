namespace MultiPosting.Application.Dto;

public class DeleteUserResourceRequest
{
    public Guid ProjectId { get; init; }
    public Guid UserResourceId { get; init; }
}