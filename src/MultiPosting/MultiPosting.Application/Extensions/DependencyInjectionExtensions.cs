using System.Reflection;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiPosting.Application.Factories;
using MultiPosting.Application.Interfaces;
using MultiPosting.Application.Options;
using MultiPosting.Application.Services;
using Shared.Application.Options;

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

        services.AddScoped<YoutubeService>();
        services.AddScoped<VkService>();
        services.AddScoped<TikTokService>();
        services.AddScoped<ISocialMediaFactory, SocialMediaFactory>();
        services.AddScoped<IProjectService, ProjectService>();
        services.Configure<YoutubeOptions>(configuration.GetSection(nameof(YoutubeOptions)));
        services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));
        services.Configure<VkOptions>(configuration.GetSection(nameof(VkOptions)));
        services.Configure<MultiPostingOptions>(configuration.GetSection(nameof(MultiPostingOptions)));
        services.Configure<TikTokOptions>(configuration.GetSection(nameof(TikTokOptions)));
        AddMappings(services);
    }

    private static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
    }
}