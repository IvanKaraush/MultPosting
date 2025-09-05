namespace MultiPosting.Application.Dto;

public class AccessTokenResponse
{
    public required string Token { get; init; }
    public required string RefreshToken { get; init; }
}