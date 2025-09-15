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
        return socialMedia switch
        {
            SocialMedia.None => _serviceProvider.GetRequiredService<AuthService>(),
            SocialMedia.YouTube => _serviceProvider.GetRequiredService<GoogleAuthService>(),
            SocialMedia.TikTok => _serviceProvider.GetRequiredService<TikTokAuthService>(),
            SocialMedia.Vk => _serviceProvider.GetRequiredService<VkAuthService>(),
            SocialMedia.Google => _serviceProvider.GetRequiredService<GoogleAuthService>(),
            _ => _serviceProvider.GetRequiredService<AuthService>()
        };
    }
}