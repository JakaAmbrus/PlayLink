namespace Catalog.Data.Entities;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public long BrandId { get; set; }
    public long CategoryId { get; set; }
    public string SKU { get; set; }
    public int InventoryQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public Brand Brand { get; set; }
    public Category Category { get; set; }
    public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    public List<ProductColor> Colors { get; set; } = new List<ProductColor>();
}