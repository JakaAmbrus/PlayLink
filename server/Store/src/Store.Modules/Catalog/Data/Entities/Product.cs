using Catalog.Common.Enums;

namespace Catalog.Data.Entities;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public string Sku { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
    
    public string ImageUrl { get; set; }
    
    public decimal Rating { get; set; }
    
    public bool IsFeatured { get; set; }
}