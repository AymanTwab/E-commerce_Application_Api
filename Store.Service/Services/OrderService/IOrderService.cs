using Store.Data.Entities;
using Store.Service.Services.OrderService.Dtos;

namespace Store.Service.Services.OrderService
{
    public interface IOrderService
    {
        public Task<OrderResultDto> CreateOrderAsync(OrderDto input);
        public Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail);
        public Task<OrderResultDto> GetOrderByIdAsync(Guid id,string BuyerEmail);
        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();



    }
}
