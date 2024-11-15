using Signal.Api.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<SignalQuery>()
    .AddMutationType<SignalMutation>()
    .AddType<SignalType>(); 

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("RestrictedCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("RestrictedCorsPolicy");

app.UseRouting();

app.MapGraphQL(); 

app.Run();
