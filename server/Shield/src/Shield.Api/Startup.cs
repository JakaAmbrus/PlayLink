using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using MediatR;
using Microsoft.OpenApi.Models;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Behaviours;
using Shield.Api.Configurations;
using Shield.Api.Endpoints;
using Shield.Api.Infrastructure.FireStoreDB;
using Shield.Api.Infrastructure.Identity;
using Shield.Api.Middleware;

namespace Shield.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Shield API",
                Version = "v1",
                Description = "API for Shield service"
            });
        });

        // Options pattern setup
        var settings = _configuration.Get<Settings>();
        services.AddSingleton(settings);
        
        // Firebase setup
        var googleCredential = GoogleCredential.FromFile(settings.Firebase.ServiceAccountKeyPath);
        
        FirebaseApp.Create(new AppOptions
        {
            Credential = googleCredential
        });

        // Create a FirestoreClient using the GoogleCredential
        var firestoreClient = new FirestoreClientBuilder
        {
            ChannelCredentials = googleCredential.ToChannelCredentials()
        }.Build();

        // Initialize FirestoreDb with FirestoreClient
        services.AddSingleton(_ => FirestoreDb.Create(settings.Firebase.ProjectId, firestoreClient));
        
        // Mediatr pipeline
        var assembly = typeof(Startup).Assembly;
        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddSingleton<IIdentityService, IdentityService>();
        services.AddSingleton<IFirebaseDbContext, FirebaseDbContext>();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHttpsRedirection();
        app.UseRouting();
        
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapSignUpEndpoint();
        });
    }
}
