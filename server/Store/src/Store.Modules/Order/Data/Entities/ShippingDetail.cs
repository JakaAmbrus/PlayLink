namespace Order.Data.Entities;

public class ShippingDetail
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string ShippingAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public Order Order { get; set; }
}