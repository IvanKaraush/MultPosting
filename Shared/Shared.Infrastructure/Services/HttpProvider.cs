using System.Net.Http.Json;
using System.Text.Json;
using Shared.Infrastructure.Interfaces;

namespace Shared.Infrastructure.Services;

public class HttpProvider : IHttpProvider
{
    public async Task<TResponse?> SendGetAsync<TResponse>(
        string url,
        CancellationToken cancellationToken,
        IDictionary<string, string>? headers = null,
        JsonSerializerOptions? options = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        using (var client = new HttpClient())
        {
            using var response = await client.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return options == null ? JsonSerializer.Deserialize<TResponse>(jsonContent)
                : JsonSerializer.Deserialize<TResponse>(jsonContent, options);        }
    }

    public async Task<TResponse?> SendPostAsync<TContent, TResponse>(string url, TContent content,
        CancellationToken cancellationToken, IDictionary<string, string>? headers = null, JsonSerializerOptions? options = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(content)
        };
        using (var client = new HttpClient())
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return options == null ? JsonSerializer.Deserialize<TResponse>(jsonContent)
                    : JsonSerializer.Deserialize<TResponse>(jsonContent, options);
        }
    }
}