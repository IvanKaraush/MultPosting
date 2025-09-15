using Microsoft.Extensions.DependencyInjection;
using MultiPosting.Application.Interfaces;
using MultiPosting.Application.Services;
using Share.Application.Enums;

namespace MultiPosting.Application.Factories;

public class SocialMediaFactory : ISocialMediaFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SocialMediaFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISocialMediaService GetSocialMediaService(SocialMedia socialMedia)
    {
        switch (socialMedia)
        {
            case SocialMedia.None:
                break;
            case SocialMedia.YouTube:
                return _serviceProvider.GetRequiredService<YoutubeService>();
            case SocialMedia.Instagram or SocialMedia.TikTok:
                break;
            case SocialMedia.Vk:
                return _serviceProvider.GetRequiredService<VkService>();
        }

        throw new ArgumentOutOfRangeException(nameof(socialMedia), socialMedia, null);
    }
}