using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.OrderEntities;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Store.API.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string endpointSecret = "whsec_d049f01af65ee86194dfd73fcb05b8b3f7d0873f78ac0e3b0dec550eb9c8f120";

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
            => Ok(await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(input));

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
            => Ok(await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId));

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentStatus(paymentIntent.Id,OrderPaymentStatus.Failed);
                    _logger.LogInformation("Order Updated to Payment Failed ",order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentStatus(paymentIntent.Id, OrderPaymentStatus.Recived);
                    _logger.LogInformation("Order Updated to Payment Succeeded ", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
