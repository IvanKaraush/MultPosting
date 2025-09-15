using System.Reflection;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Services;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Repositories;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Share.Application.Options;

namespace IdentityServer.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IAuthServiceFactory, AuthServiceFactory>();
        services.AddScoped<AuthService>();
        services.AddScoped<GoogleAuthService>();
        services.AddScoped<VkAuthService>();
        services.AddScoped<TikTokAuthService>();
        services.AddMappings();
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<VkOptions>(configuration.GetSection(nameof(VkOptions)));
        services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));
        services.Configure<MultiPostingOptions>(configuration.GetSection(nameof(MultiPostingOptions)));
        services.Configure<TikTokOptions>(configuration.GetSection(nameof(TikTokOptions)));
    }

    private static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
    }
}