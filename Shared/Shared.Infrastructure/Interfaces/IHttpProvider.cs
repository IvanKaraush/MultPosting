using System.Text.Json;

namespace Shared.Infrastructure.Interfaces;

public interface IHttpProvider
{
    Task<TResponse?> SendGetAsync<TResponse>(
        string url,
        CancellationToken cancellationToken,
        IDictionary<string, string>? headers = null,
        JsonSerializerOptions? options = null);

    Task<TResponse?> SendPostAsync<TContent, TResponse>(string url, TContent content,
        CancellationToken cancellationToken, IDictionary<string, string>? headers = null,
        JsonSerializerOptions? options = null);
}