namespace Shared.Infrastructure.Interfaces;

public interface IHttpProvider
{
    Task<TResponse?> SendPostAsync<TContent, TResponse>(string url, TContent content,
        CancellationToken cancellationToken, IDictionary<string, string>? headers = null);
}