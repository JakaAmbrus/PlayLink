using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using MediatR;
using Microsoft.OpenApi.Models;
using Shared.Core;
using Shared.Grpc;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Behaviours;
using Shield.Api.Configurations;
using Shield.Api.Endpoints;
using Shield.Api.Infrastructure.FireStoreDB;
using Shield.Api.Infrastructure.GrpcSocial;
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
        // General setup
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

        // FirebaseAuth setup
        var firestoreClient = new FirestoreClientBuilder
        {
            ChannelCredentials = googleCredential.ToChannelCredentials()
        }.Build();

        // FirestoreDb setup
        services.AddSingleton(_ => FirestoreDb.Create(settings.Firebase.ProjectId, firestoreClient));
        
        // Mediatr pipeline
        var assembly = typeof(Startup).Assembly;
        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Shared security
        services.AddSharedSecurity(settings.Firebase.ProjectId);

        // Service registration
        services.AddSingleton<IIdentityService, IdentityService>();
        services.AddSingleton<IFirebaseDbContext, FirebaseDbContext>();
        
        // Grpc client registration
        services.AddGrpcClient<UserRegistration.UserRegistrationClient>(options =>
        {
            options.Address = new Uri(settings.SocialUrl);
        });

        services.AddScoped<ISocialClientService, SocialClientService>();
        
        // CORS setup
        services.AddCors(options =>
        {
            options.AddPolicy("RestrictedCorsPolicy", policy =>
            {
                policy.WithOrigins(settings.AllowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        
        app.UseCors("RestrictedCorsPolicy");

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapSignUpEndpoint();
        });
    }
}
