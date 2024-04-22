﻿namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public string BuyerEmail { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
