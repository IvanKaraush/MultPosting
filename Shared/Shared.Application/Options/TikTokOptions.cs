namespace Shared.Application.Options;

public class TikTokOptions
{
    public string RedirectUri { get; init; }
    public string ClientKey { get; init; }
    public string ClientSecret { get; init; }
    public string AuthorizationUrl { get; init; }
    public string UserInfoUrl { get; init; }
    public string TokenUrl { get; init; }
    public string RedirectApplicationUrl { get; init; }
}