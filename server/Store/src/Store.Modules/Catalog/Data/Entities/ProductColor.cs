namespace Catalog.Data.Entities;

public class ProductColor
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string ColorName { get; set; }
    public string HexCode { get; set; }
    public int InventoryQuantity { get; set; }

    public Product Product { get; set; }
}