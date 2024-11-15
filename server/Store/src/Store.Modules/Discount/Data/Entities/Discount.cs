namespace Discount.Data.Entities;

public class Discount
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? FixedAmount { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    
    public List<Coupon> Coupons { get; set; }
}
