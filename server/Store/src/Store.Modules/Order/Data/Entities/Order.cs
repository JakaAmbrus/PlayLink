using Order.Shared.Enums;

namespace Order.Data.Entities;

public class Order
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string StripePaymentIntentId { get; set; }
    public string StripeSessionId { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ShippingDetail ShippingDetail { get; set; }
    public Payment Payment { get; set; }
}