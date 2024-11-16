namespace Catalog.Data.Entities;

public class ProductImage
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string ImageUrl { get; set; }
    public string ImagePublicId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }

    public Product Product { get; set; }
}