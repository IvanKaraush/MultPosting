using IdentityServer.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Interfaces;
using Shared.Infrastructure.Services;

namespace IdentityServer.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddNpgsql<ApplicationIdentityDbContext>(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly(typeof(ApplicationIdentityDbContext).Assembly));

        services.AddNpgsql<ApplicationDbContext>(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly));

        services.AddScoped<IHttpProvider, HttpProvider>();
        
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();
    }
}