using IdentityServer.Application.Interfaces;
using IdentityServer.Infrastructure.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace IdentityServer.Application.Services;

public class AccessTokenService : IAccessTokenService
{
    private readonly IAccessTokenRepository _accessTokenRepository;

    public AccessTokenService(IAccessTokenRepository accessTokenRepository)
    {
        _accessTokenRepository = accessTokenRepository;
    }

    public async Task<AccessTokenResponse> GetAccessTokenByEmailAsync(string email)
    {
        var token = await _accessTokenRepository.GetByFilterAsync(c => c.Email == email);
        return token.Adapt<AccessTokenResponse>();
    }
}