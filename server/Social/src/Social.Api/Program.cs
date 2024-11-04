using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Core;
using Social.Api.Extensions;
using Social.Api.Filters;
using Social.Api.Grpc;
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
builder.Services.AddGrpc();

builder.Services.AddScoped<IUserActivityService, UserActivityService>();
builder.Services.AddScoped<LogUserActivity>();

builder.WebHost.ConfigureKestrel(options =>
{
    var httpPort = builder.Configuration.GetValue<int>("Port");
    var grpcPort = builder.Configuration.GetValue<int>("GrpcPort");
    
    options.ListenAnyIP(httpPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    
    options.ListenAnyIP(grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("RestrictedCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


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

app.UseCors("RestrictedCorsPolicy");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("RestrictedCorsPolicy");

app.MapGrpcService<UserRegistrationService>();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

public partial class Program { } // for testing purposes