using WebAPI.Middleware;
using Application;
using Infrastructure;
using Serilog;
using Infrastructure.Extensions;
using Infrastructure.ExternalServices;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

/*builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
*/
var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

ApplicationStartupExtensions.ConfigureApplication(app, app.Services);


app.MapControllers();

app.Run();
