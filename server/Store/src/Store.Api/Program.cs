using System.Reflection;
using Basket;
using Catalog;
using Discount;
using FastEndpoints;
using FastEndpoints.Swagger;
using Order;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting api configuration");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints().SwaggerDocument();

List<Assembly> mediatorAssemblies = [typeof(Program).Assembly];
builder.Services
    .AddBasketModuleServices(builder.Configuration, mediatorAssemblies)
    .AddCatalogModuleServices(builder.Configuration, mediatorAssemblies)
    .AddDiscountModuleServices(builder.Configuration, mediatorAssemblies)
    .AddOrderModuleServices(builder.Configuration, mediatorAssemblies);

logger.Information("Module registrations finished");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
