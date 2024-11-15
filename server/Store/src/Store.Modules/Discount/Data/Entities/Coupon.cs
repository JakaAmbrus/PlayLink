namespace Discount.Data.Entities;

public class Coupon
{
    public long Id { get; set; }
    public string Code { get; set; }
    public long DiscountId { get; set; }
    public long? UserId { get; set; }
    public bool IsActive { get; set; }
    public int? UsageLimit { get; set; }
    public int UsageCount { get; set; }
    public DateTime ExpirationDate { get; set; }

    public Discount Discount { get; set; }
}