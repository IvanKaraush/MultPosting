namespace Share.Application.Options;

public class GoogleOptions
{
    public required string ClientSecret { get; init; }
    public required string ClientId { get; init; }
    public required string RedirectUri { get; init; }
    public required string AuthorizationUrl { get; init; }
    public required string TokenUrl { get; init; }
    public required string UserInfoUrl { get; init; }
}