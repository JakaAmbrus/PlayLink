using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Common;

[AllowAnonymous]
public abstract class CatalogBaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull
{
    protected const string BaseRoute = "api/catalog";

    private ISender _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected async Task SendResultAsync(Result<TResponse> result, CancellationToken cancellationToken)
    {
        if (result.Status != ResultStatus.Ok)
        {
            await SendResultAsync(result.ToMinimalApiResult());
        }
        else
        {
            await SendAsync(result.Value, cancellation: cancellationToken);
        }
    }
}