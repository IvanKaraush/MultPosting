using Shared.Application.Enums;

namespace MultiPosting.Application.Interfaces;

public interface ISocialMediaFactory
{
    ISocialMediaService GetSocialMediaService(SocialMedia socialMedia);
}