using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Shield.Api.Features.SignUp;

public static class SignUpEndpoint
{
    public static void MapSignUpEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/signup", async ([FromServices] ISender mediator, SignUpCommand request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            });
    }
}