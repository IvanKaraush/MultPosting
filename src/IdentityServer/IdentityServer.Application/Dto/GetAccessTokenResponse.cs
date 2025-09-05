namespace IdentityServer.Application.Dto;

public class GetAccessTokenResponse
{
    public required string Token { get; init; }
    public required string RefreshToken { get; init; }
}