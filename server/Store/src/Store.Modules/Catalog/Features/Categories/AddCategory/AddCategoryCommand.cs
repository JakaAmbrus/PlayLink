using Store.Shared.Abstractions;

namespace Catalog.Features.Categories.AddCategory;

public class AddCategoryCommand : ICommand<AddCategoryResponse>
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public long? ParentCategoryId { get; set; }
}