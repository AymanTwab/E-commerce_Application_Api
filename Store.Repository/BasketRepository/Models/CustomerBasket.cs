namespace Store.Repository.BasketRepository.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal DeliveryPeice { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

    }
}
