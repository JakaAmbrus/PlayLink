using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Security;
using Shield.Api.Features.DeleteAccount;

namespace Shield.Api.Endpoints;

public static class DeleteAccountEndpoint
{
    public static void MapDeleteAccountEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/shield/account", async ([FromServices] ISender mediator, [FromServices] IAuthContextService authContextService, CancellationToken cancellationToken) =>
            {
                var command = new DeleteAccountCommand
                {
                    UserId = authContextService.GetUserId(),
                };
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization("Member", "DenyGuestRole")
            .RequireCors("RestrictedCorsPolicy")
            .WithOpenApi();
    }
}