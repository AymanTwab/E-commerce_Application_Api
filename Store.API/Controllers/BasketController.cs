using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;

namespace Store.API.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
            => Ok(await _basketService.GetBasketAsync(id));

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketById(CustomerBasketDto customerBasket)
            => Ok(await _basketService.UpdateBasketAsync(customerBasket));

        [HttpDelete]
        public async Task<ActionResult> DeleteBasketById(string id)
            => Ok(await _basketService.DeleteBasketAsync(id));


    }
}
