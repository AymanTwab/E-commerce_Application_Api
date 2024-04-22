namespace Store.Data.Entities.OrderEntities
{
    public enum OrderPaymentStatus
    {
        Pending,
        Recived,
        Failed
    }
    public class Order : BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? BasketId { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Price;

    }
}
