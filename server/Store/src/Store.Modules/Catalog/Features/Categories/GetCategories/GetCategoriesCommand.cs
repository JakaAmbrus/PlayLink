using Store.Shared.Abstractions;

namespace Catalog.Features.Categories.GetCategories;

public class GetCategoriesCommand : ICommand<GetCategoriesResponse>
{
    public string Bla { get; set; }
}