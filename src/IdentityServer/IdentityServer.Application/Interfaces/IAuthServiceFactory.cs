using Share.Application.Enums;

namespace IdentityServer.Application.Interfaces;

public interface IAuthServiceFactory
{
    public IAuthService GetAuthService(SocialMedia socialMedia);
}