using Catalog.Features.CreateProduct;
using FastEndpoints;

namespace Catalog.Endpoints.CreateProduct;

internal class CreateProductSummary : Summary<CreateProductEndpoint>
{
    public CreateProductSummary()
    {
        Summary = "Creates a new product";
        Description = "This endpoint allows the creation of a new product in the catalog.";
        ExampleRequest = new CreateProductCommand { Name = "Sample Product", Bla = "SDFDS"};

        Response(200, "Product successfully created", example: new CreateProductResponse { Name = "Sample Product" });
        Response(404, "Product not found");
        Response(400, "Invalid request data");
    }
}