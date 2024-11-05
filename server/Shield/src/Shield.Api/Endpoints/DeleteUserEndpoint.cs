using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shield.Api.Features.DeleteUser;

namespace Shield.Api.Endpoints;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/shield", async ([FromServices] ISender mediator, DeleteUserCommand request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization("Admin")
            .RequireAuthorization("DenyGuestRole")
            .RequireCors("RestrictedCorsPolicy")
            .WithOpenApi();
    }
}