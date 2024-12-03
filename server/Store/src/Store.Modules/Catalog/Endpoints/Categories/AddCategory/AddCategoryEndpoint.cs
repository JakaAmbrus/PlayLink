using Catalog.Endpoints.CatalogBase;
using Catalog.Features.Categories.AddCategory;
using FastEndpoints;

namespace Catalog.Endpoints.Categories.AddCategory;

[HttpPost($"{BaseRoute}/categories/")]
internal class AddCategoryEndpoint : CatalogBaseEndpoint<AddCategoryCommand, AddCategoryResponse>
{
    public override async Task HandleAsync(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        await SendResultAsync(result, cancellationToken);
    }
}