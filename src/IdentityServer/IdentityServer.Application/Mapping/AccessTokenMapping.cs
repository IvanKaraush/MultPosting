using IdentityServer.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace IdentityServer.Application.Mapping;

public class AccessTokenMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AccessToken, AccessTokenResponse>().TwoWays();
    }
}