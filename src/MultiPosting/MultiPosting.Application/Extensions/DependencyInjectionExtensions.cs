using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiPosting.Application.Interfaces;
using MultiPosting.Application.Options;
using MultiPosting.Application.Services;

namespace MultiPosting.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var youtubeApiKey = configuration.GetSection(nameof(YoutubeOptions)).Get<YoutubeOptions>() ??
                            throw new ArgumentException(nameof(YoutubeOptions));

        services.AddSingleton(_ => new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = youtubeApiKey.ApiKey,
            ApplicationName = "Multiposting"
        }));

        services.AddScoped<IYoutubeService, YoutubeService>();
    }
}