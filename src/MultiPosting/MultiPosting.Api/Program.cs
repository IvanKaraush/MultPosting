using MultiPosting.Application.Extensions;
using MultiPosting.Application.Options;
using MultiPosting.Infrastructure.Extensions;
using Share.Application.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.RegisterApplication(builder.Configuration);
builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.Configure<YoutubeOptions>(builder.Configuration.GetSection(nameof(YoutubeOptions)));
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection(nameof(GoogleOptions)));
builder.Services.Configure<MultiPostingOptions>(builder.Configuration.GetSection(nameof(MultiPostingOptions)));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config => { config.EnableDeepLinking(); });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();