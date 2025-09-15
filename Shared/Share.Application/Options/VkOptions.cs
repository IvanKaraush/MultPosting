namespace Share.Application.Options;

public class VkOptions
{
    public string ClientSecret { get; init; }
    public int ApplicationId { get; init; }
    public string RedirectUrl { get; init; }
    public string TokenUrl { get; init; }
    public string UserInfoUrl { get; init; }
    public string Version { get; init; }
}