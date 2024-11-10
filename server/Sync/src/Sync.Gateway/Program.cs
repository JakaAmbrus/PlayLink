using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configDirectory = Path.Combine(Directory.GetCurrentDirectory(), env.EnvironmentName);
builder.Configuration.AddOcelot(configDirectory, builder.Environment);

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OcelotCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("OcelotCorsPolicy");

app.UseWebSockets();

if (env.IsProduction())
{
    app.UseHttpsRedirection();
}

await app.UseOcelot();

app.Run();