using MultiPosting.Application.Extensions;
using MultiPosting.Application.Options;

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
builder.Services.Configure<YoutubeOptions>(builder.Configuration.GetSection(nameof(YoutubeOptions)));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config => { config.EnableDeepLinking(); });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();