using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shield.Api.Features.SignInGuest;

namespace Shield.Api.Endpoints;

public static class SignInGuestEndpoint
{
    public static void MapSignInGuestEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/shield/guest", async ([FromServices] ISender mediator, SignInGuestCommand request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            })
            .RequireCors("RestrictedCorsPolicy")
            .WithOpenApi();
    }
}