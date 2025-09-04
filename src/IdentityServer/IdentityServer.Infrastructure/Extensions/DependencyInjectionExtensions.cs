using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddNpgsql<ApplicationIdentityDbContext>(configuration.GetConnectionString("DefaultConnection"), 
            x => x.MigrationsAssembly(typeof(ApplicationIdentityDbContext).Assembly));
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();
    }
}