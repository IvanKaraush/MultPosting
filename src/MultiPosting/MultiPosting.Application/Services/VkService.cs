using System.Text.Json;
using Microsoft.Extensions.Options;
using MultiPosting.Application.Dto;
using MultiPosting.Application.Interfaces;
using Shared.Application.Options;
using Shared.Infrastructure.Interfaces;

namespace MultiPosting.Application.Services;

public class VkService : IVkService
{
    private readonly VkOptions _vkOptions;
    private readonly MultiPostingOptions _multiPostingOptions;
    private readonly IHttpProvider _httpProvider;

    public VkService(IOptions<VkOptions> vkOptions, IHttpProvider httpProvider,
        IOptions<MultiPostingOptions> multiPostingOptions)
    {
        _httpProvider = httpProvider;
        _multiPostingOptions = multiPostingOptions.Value;
        _vkOptions = vkOptions.Value;
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

        var url = $"https://api.vk.com/method/groups.get" +
                  $"?access_token={token.Token}" +
                  $"&v={_vkOptions.Version}" +
                  $"&filter=admin" +
                  $"&fields=photo_100,photo_max&" +
                  $"&extended=1";
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("error", out var error))
            {
                var errorCode = error.GetProperty("error_code").GetInt32();
                var errorMsg = error.GetProperty("error_msg").GetString();
                throw new Exception($"VK API error: {errorCode} - {errorMsg}");
            }

            if (doc.RootElement.TryGetProperty("response", out var resp))
            {
                var items = resp.GetProperty("items");
                var groups = new List<UserResourceDto>();
                foreach (var item in items.EnumerateArray())
                {
                    var g = new UserResourceDto
                    {
                        Name = item.GetProperty("name").GetString(),
                        ImageUrl = item.GetProperty("photo_max").GetString()
                    };
                    groups.Add(g);
                }

                return groups.ToArray();
            }
        }

        return [];
    }
}