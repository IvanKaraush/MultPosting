using System.Text.Json.Serialization;
using MultiPosting.Application.Extensions;
using MultiPosting.Infrastructure.Extensions;
using MultiPosting.Infrastructure.Interfaces;
using MultPosting.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.RegisterApplication(builder.Configuration);
builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config => { config.EnableDeepLinking(); });
}

app.Lifetime.ApplicationStarted.Register(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
        var projectId = Guid.Parse("6C22B3EF-C512-485D-877F-F7E67D3F2E5E");
        var project = await projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            await projectRepository.AddAsync(new Project(projectId, "Test"));
            await projectRepository.SaveChangesAsync();
        }
    }
});

app.MapControllers();

app.Run();