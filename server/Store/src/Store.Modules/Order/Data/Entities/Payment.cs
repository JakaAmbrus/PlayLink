using Order.Shared.Enums;

namespace Order.Data.Entities;

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string StripePaymentIntentId { get; set; }
    public string StripeChargeId { get; set; }

    public Order Order { get; set; }
}