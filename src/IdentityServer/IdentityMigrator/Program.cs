using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

const string defaultEnvironmentName = "Development";
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? defaultEnvironmentName;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.local.json",
                optional: true);
        builder.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) => { services.RegisterInfrastructure(context.Configuration); })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

using var scope = host.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    var applicationIdentityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await applicationIdentityDbContext.Database.MigrateAsync();
    await applicationDbContext.Database.MigrateAsync();

    logger.LogInformation("Миграции успешно применены.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Ошибка при выполнении миграций");
}