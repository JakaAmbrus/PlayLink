using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configDirectory = Path.Combine(Directory.GetCurrentDirectory(), env.EnvironmentName);
builder.Configuration.AddOcelot(configDirectory, builder.Environment);

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (env.IsProduction())
{
    app.UseHttpsRedirection();
}

await app.UseOcelot();

app.Run();