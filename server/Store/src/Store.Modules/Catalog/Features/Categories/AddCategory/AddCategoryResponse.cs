namespace Catalog.Features.Categories.AddCategory;

public class AddCategoryResponse
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public long? ParentCategoryId { get; set; }
}