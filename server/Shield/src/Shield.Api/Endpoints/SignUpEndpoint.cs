using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shield.Api.Features.SignUp;

namespace Shield.Api.Endpoints;

public static class SignUpEndpoint
{
    public static void MapSignUpEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/shield/signup", async ([FromServices] ISender mediator, SignUpCommand request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            }).WithOpenApi().RequireCors("RestrictedCorsPolicy");
    }
}