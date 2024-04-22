using Store.Data.Entities.OrderEntities;

namespace Store.Repository.Specification.OrderService
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string? PaymentIntentId)
            : base(order => order.PaymentIntentId == PaymentIntentId)
        {
        }
    }
}
