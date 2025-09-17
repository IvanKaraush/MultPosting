using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiPosting.Infrastructure.Interfaces;
using MultiPosting.Infrastructure.Repositories;
using Shared.Infrastructure.Interfaces;
using Shared.Infrastructure.Services;

namespace MultiPosting.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IHttpProvider, HttpProvider>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddNpgsql<MultiPostingDbContext>(configuration.GetConnectionString("DefaultConnection"));
    }
}