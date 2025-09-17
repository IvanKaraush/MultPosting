using Shared.Application.Enums;

namespace MultiPosting.Application.Dto;

public class GetUserInfoRequest
{
    public string Login { get; init; }
    public SocialMedia SocialMedia { get; init; }
}