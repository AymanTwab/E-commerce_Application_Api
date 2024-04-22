using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderService;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Stripe;
using Product = Store.Data.Entities.Product;

namespace Store.Service.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(
            IConfiguration configuration,
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _configuration = configuration;
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            if (basket is null)
                throw new Exception("Basket Is Empty");

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            var shippingPrice = deliveryMethod.Price;

            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => (item.Quantity * (item.Price * 100))) + (shippingPrice * 100)),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => (item.Quantity * (item.Price * 100))) + (shippingPrice * 100))
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketService.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null)
                throw new Exception("Basket Is Empty");

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            var shippingPrice = deliveryMethod.Price;

            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => (item.Quantity * (item.Price * 100))) + (shippingPrice * 100)),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => (item.Quantity * (item.Price * 100))) + (shippingPrice * 100))
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketService.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentStatus(string paymentIntentId, OrderPaymentStatus status)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order, Guid>().GetByIdWithSpecificationsAsync(specs);

            if (order is null)
                throw new Exception("Order is Not Exist");
            if(status == OrderPaymentStatus.Failed)
            {
                order.OrderPaymentStatus = OrderPaymentStatus.Failed;
            }
            else if (status == OrderPaymentStatus.Recived)
            {
                order.OrderPaymentStatus = OrderPaymentStatus.Recived;
                await _basketService.DeleteBasketAsync(order.BasketId);
            }

            _unitOfWork.Repository<Order, Guid>().Update(order);
            await _unitOfWork.CompleteAsync();
            var mappedOrder = _mapper.Map<OrderResultDto>(order);


            return mappedOrder;
        }
    }
}
