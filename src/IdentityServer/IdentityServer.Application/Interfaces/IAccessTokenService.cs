using IdentityServer.Application.Dto;

namespace IdentityServer.Application.Interfaces;

public interface IAccessTokenService
{
    Task<GetAccessTokenResponse> GetAccessTokenByEmailAsync(string email);
}