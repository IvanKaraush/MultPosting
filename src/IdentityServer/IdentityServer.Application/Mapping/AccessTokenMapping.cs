using IdentityServer.Application.Dto;
using IdentityServer.Domain.Entities;
using Mapster;

namespace IdentityServer.Application.Mapping;

public class AccessTokenMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AccessToken, GetAccessTokenResponse>().TwoWays();
    }
}