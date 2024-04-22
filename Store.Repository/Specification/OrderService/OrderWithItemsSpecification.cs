using Store.Data.Entities.OrderEntities;

namespace Store.Repository.Specification.OrderService
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail )
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
            
        }

        public OrderWithItemsSpecification(Guid id,string buyerEmail)
            : base(order => order.BuyerEmail == buyerEmail && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
