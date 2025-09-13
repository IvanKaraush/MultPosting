using System.Text.Json;
using Microsoft.Extensions.Options;
using MultiPosting.Application.Dto;
using Share.Application.Options;

namespace MultiPosting.Application.Services;

public class VkService
{
    private readonly VkOptions _vkOptions;

    public VkService(IOptions<VkOptions> vkOptions)
    {
        _vkOptions = vkOptions.Value;
    }


    public async Task<VkGroupDto[]> GetGroupsWhereUserIsAdminAsync()
    {
        var url = $"https://api.vk.com/method/groups.get" +
                  $"?access_token={_accessToken}" +
                  $"&v={_apiVersion}" +
                  $"&filter=admin" +
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
                var groups = new List<VkGroupDto>();
                foreach (var item in items.EnumerateArray())
                {
                    var g = new VkGroupDto
                    {
                        Id = item.GetProperty("id").GetInt64(),
                        Name = item.GetProperty("name").GetString(),
                        ScreenName = item.GetProperty("screen_name").GetString(),
                        IsClosed = item.GetProperty("is_closed").GetInt32(),
                        Type = item.GetProperty("type").GetString()
                    };
                    groups.Add(g);
                }

                return groups.ToArray();
            }

        }
       
        return [];
    }
}