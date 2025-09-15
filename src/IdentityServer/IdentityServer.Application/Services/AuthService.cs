using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Application.Services;

public class AuthService : BaseAuthService, IAuthService
{
    public AuthService(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions) : base(userManager, jwtOptions)
    {
    }

    public string GetAuthorizationUrl()
    {
        return string.Empty;
    }

    public async Task<string> SignUpAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        return await CreateUserAsync(request.Email, request.Password, cancellationToken);
    }

    public Task<string> SignInAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> SignUpRedirectAsync(AuthRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(string.Empty);
    }

    public Task<string> SignUpRedirectAsync()
    {
        return Task.FromResult(string.Empty);
    }
}