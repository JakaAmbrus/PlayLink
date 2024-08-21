using Social.Application;
using Social.Application.Interfaces;
using Social.Infrastructure;
using Social.Infrastructure.Data;
using Social.Infrastructure.Extensions;
using Social.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Social.Api.Extensions;
using Social.Api.Filters;
using Social.Api.Middleware;
using Social.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserActivityService, UserActivityService>();

builder.Services.AddScoped<LogUserActivity>();

builder.Services.AddSignalRExtensions();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// To make it easier to spin up the docker compose file I added this so anybody can run the site
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSerilogRequestLogging();

ApplicationStartupExtensions.ConfigureApplication(app, app.Services);

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

public partial class Program { } // for testing purposes