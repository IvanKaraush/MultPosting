using System.Text.Json.Serialization;
using MultiPosting.Application.Extensions;
using MultiPosting.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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

app.MapControllers();

app.Run();