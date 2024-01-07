using Application;
using Application.Interfaces;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Filters;
using WebAPI.Middleware;
using WebAPI.SignalR;

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

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "text/plain", "text/css", "application/javascript", "text/html", "application/json" });
});

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

ApplicationStartupExtensions.ConfigureApplication(app, app.Services);

app.UseResponseCompression();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToController("Index", "Fallback");

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

public partial class Program { }