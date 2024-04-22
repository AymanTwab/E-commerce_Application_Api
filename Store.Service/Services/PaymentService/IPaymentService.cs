using Store.Data.Entities.OrderEntities;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;

namespace Store.Service.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input);
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentStatus(string paymentIntentId, OrderPaymentStatus status);
    }
}
