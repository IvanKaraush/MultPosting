using IdentityServer.Application.Interfaces;
using IdentityServer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}