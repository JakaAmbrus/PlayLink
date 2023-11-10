using WebAPI.Middleware;
using Application;
using Infrastructure;
using Serilog;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

/*app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();*/
ApplicationStartupExtensions.ConfigureApplication(app, app.Services);

app.MapControllers();

app.Run();
