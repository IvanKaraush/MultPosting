using IdentityServer.Application.Dto;
using IdentityServer.Application.Interfaces;
using IdentityServer.Infrastructure.Interfaces;
using Mapster;

namespace IdentityServer.Application.Services;

public class AccessTokenService : IAccessTokenService
{
    private readonly IAccessTokenRepository _accessTokenRepository;

    public AccessTokenService(IAccessTokenRepository accessTokenRepository)
    {
        _accessTokenRepository = accessTokenRepository;
    }

    public async Task<GetAccessTokenResponse> GetAccessTokenByEmailAsync(string email)
    {
        var token = await _accessTokenRepository.GetByFilterAsync(c => c.Email == email);
        return token.Adapt<GetAccessTokenResponse>();
    }
}