using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.ProductService;
using Store.Service.Helper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Store.API.Controllers
{
    //[Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDetailsDto>>> GetAllCategories()
        => Ok(await _productService.GetAllCategoriesAsync());

        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification productSpecs)
            => Ok(await _productService.GetAllProductAsync(productSpecs));

        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(int? id)
            => Ok(await _productService.GetProductByIdAsync(id));
    }
}
