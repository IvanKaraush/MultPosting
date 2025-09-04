namespace IdentityServer.Application.Interfaces;

public interface IAuthService
{
    Task<string> CreateUserAsync(string username, string password);
    Task<string> GenerateJwtTokenAsync(string username, string password);
}