using System.Text.Json;
using Microsoft.Extensions.Options;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;
using Shared.Application.Options;
using Shared.Infrastructure.Interfaces;

namespace MultiPosting.Application.Services;

public class TikTokService : ISocialMediaService
{
    private readonly MultiPostingOptions _multiPostingOptions;
    private readonly TikTokOptions _tikTokOptions;
    private readonly IHttpProvider _httpProvider;

    public TikTokService(IHttpProvider httpProvider, IOptions<MultiPostingOptions> multiPostingOptions,
        IOptions<TikTokOptions> tikTokOptions)
    {
        _httpProvider = httpProvider;
        _tikTokOptions = tikTokOptions.Value;
        _multiPostingOptions = multiPostingOptions.Value;
    }

    public async Task<ICollection<UserResourceDto>> GetUserResourceAsync(string login)
    {
        var token = await _httpProvider.SendGetAsync<AccessTokenResponse>(
            $"{_multiPostingOptions.IdentityServerUrl}/api/accesstoken?email={login}", CancellationToken.None,
            options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        if (token == null)
        {
            return [];
        }

        var headers = new Dictionary<string, string>
        {
            ["Authorization"] = $"Bearer {token.Token}"
        };

        var response = await _httpProvider.SendGetAsync<TikTokUserInfoResponse>(
            $"{_tikTokOptions.UserInfoUrl}?fields={Uri.EscapeDataString("avatar_url,display_name")}",
            CancellationToken.None,
            headers, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
        return new List<UserResourceDto>()
        {
            new()
            {
                Name = response.Data.User.DisplayName,
                ImageUrl = response.Data.User.AvatarUrl
            }
        };
    }
}