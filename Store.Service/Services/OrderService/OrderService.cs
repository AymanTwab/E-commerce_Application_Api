using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderService;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket == null)
                throw new Exception("Basket Not Exist");

            var orderItems = new List<OrderItemDto>();

            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (productItem == null)
                    throw new Exception($"Product With Id : {basketItem.ProductId} Not Exist");

                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.ImageUrl
                };

                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    ItemOrdered = itemOrdered
                };

                var MappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);

                orderItems.Add(MappedOrderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if (deliveryMethod == null)
                throw new Exception("Delivery Method Not Provided");

            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);

            //--
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetByIdWithSpecificationsAsync(specs);
            if(existingOrder is not null)
            {
                _unitOfWork.Repository<Order, Guid>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);

            }

            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                BuyerEmail = input.BuyerEmail,
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                OrderItems = mappedOrderItems,
                SubTotal = subTotal,
                BasketId = basket.Id,
                PaymentIntentId = basket.PaymentIntentId
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
            => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(buyerEmail);

            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllSpecificationsAsync(specs);

            if (orders is { Count: <= 0 })
                throw new Exception("You Do Not Have Any Orders");

            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id, buyerEmail);

            var order = await _unitOfWork.Repository<Order, Guid>().GetByIdWithSpecificationsAsync(specs);

            if (order is null)
                throw new Exception($"There Is No Order With Id: {id}");

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}
