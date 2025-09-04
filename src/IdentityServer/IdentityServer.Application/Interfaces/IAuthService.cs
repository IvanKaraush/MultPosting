namespace IdentityServer.Application.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(string username, string password);
}