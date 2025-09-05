namespace IdentityServer.Application.Interfaces;

public interface IAuthService
{
    Task<string> GoogleSignUpAsync(string code);
    Task<string> CreateUserAsync(string email, string password);
    Task<string> GenerateJwtTokenAsync(string email, string password);
    string GenerateAuthorizationUrl();
}