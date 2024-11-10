using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shield.Api.Features.EditRole;

namespace Shield.Api.Endpoints;

public static class EditRoleEndpoint
{
    public static void MapEditRoleEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/shield/edit-role", async ([FromServices] ISender mediator, EditRoleCommand request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization("Admin", "DenyGuestRole")
            .WithOpenApi();
    }
}