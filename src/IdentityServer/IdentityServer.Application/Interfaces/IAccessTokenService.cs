using Microsoft.AspNetCore.Authentication.BearerToken;

namespace IdentityServer.Application.Interfaces;

public interface IAccessTokenService
{
    Task<AccessTokenResponse> GetAccessTokenByEmailAsync(string email);
}