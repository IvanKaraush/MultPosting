using IdentityServer.Application.Dto;

namespace IdentityServer.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignUpAsync(AuthRequest request, CancellationToken cancellationToken);
    Task<string> SignInAsync();
    Task<string> SignUpRedirectAsync(AuthRequest request, CancellationToken cancellationToken);
}