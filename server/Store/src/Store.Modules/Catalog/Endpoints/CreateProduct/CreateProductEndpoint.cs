using Catalog.Common;
using Catalog.Features.CreateProduct;
using FastEndpoints;

namespace Catalog.Endpoints.CreateProduct;

[HttpPost($"{BaseRoute}/product")]
internal class CreateProductEndpoint : CatalogBaseEndpoint<CreateProductCommand, CreateProductResponse>
{
    public override async Task HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);

        await SendResultAsync(result, cancellationToken);
    }
}