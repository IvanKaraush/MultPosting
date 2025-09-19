using Shared.Domain.Enums;

namespace IdentityServer.Application.Interfaces;

public interface IAuthServiceFactory
{
    public IAuthService GetAuthService(SocialMedia socialMedia);
}