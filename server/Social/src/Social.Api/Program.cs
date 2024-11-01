using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Core;
using Social.Api.Extensions;
using Social.Api.Filters;
using Social.Api.Middleware;
using Social.Api.SignalR;
using Social.Application;
using Social.Application.Interfaces;
using Social.Infrastructure;
using Social.Infrastructure.Data;
using Social.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddSharedSecurity(builder.Configuration["FirebaseProjectId"]!);

builder.Services.AddSignalRExtensions();

builder.Services.AddScoped<IUserActivityService, UserActivityService>();
builder.Services.AddScoped<LogUserActivity>();

builder.Services.AddCors();

var app = builder.Build();

// To make it easier to spin up the docker compose file I added this so anybody can run the site
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
        var scopedProvider = scope.ServiceProvider;
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors(cpb => cpb
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("https://localhost:4200", "http://localhost:4200"));
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

public partial class Program { } // for testing purposes