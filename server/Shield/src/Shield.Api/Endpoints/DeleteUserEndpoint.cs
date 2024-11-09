using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shield.Api.Features.DeleteUser;

namespace Shield.Api.Endpoints;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/shield/{userId}", async ([FromServices] ISender mediator, string userId, CancellationToken cancellationToken) =>
            {
                var command = new DeleteUserCommand
                {
                    UserId = userId,
                };
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization("Admin", "DenyGuestRole")
            .RequireCors("RestrictedCorsPolicy")
            .WithOpenApi();
    }
}