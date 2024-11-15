namespace Catalog.Data.Entities;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long? ParentCategoryId { get; set; }

    public Category ParentCategory { get; set; }
    public List<Category> SubCategories { get; set; } = new List<Category>();
}