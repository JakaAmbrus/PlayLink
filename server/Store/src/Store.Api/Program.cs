using System.Reflection;
using Basket;
using Catalog;
using Discount;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using MediatR;
using Order;
using Serilog;
using Store.Shared.Behaviours;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting api configuration");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "PlayLink Store Api";
            s.DocumentName = "Store Api";
            s.Description = "Swagger documentation for the PlayLink Store API";
        };
        o.RemoveEmptyRequestSchema = true;
        o.AutoTagPathSegmentIndex = 2;
        o.TagDescriptions = tags =>
        {
            tags["Catalog"] = "Catalog Module Endpoints";
            tags["Basket"] = "Basket Module Endpoints";
            tags["Discount"] = "Discount Module Endpoints";
            tags["Order"] = "Order Module Endpoints";
        };
    });

List<Assembly> mediatorAssemblies = [typeof(Program).Assembly];
builder.Services
    .AddBasketModuleServices(builder.Configuration, mediatorAssemblies)
    .AddCatalogModuleServices(builder.Configuration, mediatorAssemblies)
    .AddDiscountModuleServices(builder.Configuration, mediatorAssemblies)
    .AddOrderModuleServices(builder.Configuration, mediatorAssemblies);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(mediatorAssemblies.ToArray()));
builder.Services.AddValidatorsFromAssemblies(mediatorAssemblies);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseDefaultExceptionHandler();

app.UseHttpsRedirection();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
