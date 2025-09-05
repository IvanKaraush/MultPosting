using System.Reflection;
using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Services;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Repositories;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddMappings();
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
    }

    private static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
    }
}