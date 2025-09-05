using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Interfaces;
using Shared.Infrastructure.Services;

namespace MultiPosting.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IHttpProvider, HttpProvider>();
    }
}