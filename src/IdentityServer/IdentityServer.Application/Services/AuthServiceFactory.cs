using IdentityServer.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Share.Application.Enums;

namespace IdentityServer.Application.Services;

public class AuthServiceFactory : IAuthServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public AuthServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IAuthService GetAuthService(SocialMedia socialMedia)
    {
        switch (socialMedia)
        {
            case SocialMedia.None:
                return _serviceProvider.GetRequiredService<AuthService>();
            case SocialMedia.YouTube:
                return _serviceProvider.GetRequiredService<GoogleAuthService>();
            case SocialMedia.Instagram:
            case SocialMedia.TikTok:
                break;
            case SocialMedia.Vk:
                return _serviceProvider.GetRequiredService<VkAuthService>();
            case SocialMedia.Google:
                return _serviceProvider.GetRequiredService<GoogleAuthService>();
            default:
                return _serviceProvider.GetRequiredService<AuthService>();
        }

        throw new ArgumentOutOfRangeException(nameof(socialMedia), socialMedia, null);
    }
}