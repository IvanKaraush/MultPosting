using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Options;
using IdentityServer.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
    }
}