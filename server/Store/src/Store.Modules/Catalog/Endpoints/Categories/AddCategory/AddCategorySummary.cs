using Catalog.Features.Categories.AddCategory;
using FastEndpoints;

namespace Catalog.Endpoints.Categories.AddCategory;

internal class AddCategorySummary : Summary<AddCategoryEndpoint>
{
    public AddCategorySummary()
    {
        Summary = "Creates a new category";
        Description = "This endpoint allows the creation of a new category.";
        ExampleRequest = new AddCategoryCommand { };

        Response(200, "Successfully created", example: new AddCategoryResponse { });
        Response(404, "Not found");
        Response(400, "Invalid request data");
    }
}