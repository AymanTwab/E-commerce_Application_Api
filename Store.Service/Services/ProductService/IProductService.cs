using Store.Repository.Specification.ProductService;
using Store.Service.Helper;
using Store.Service.Services.ProductService.Dtos;

namespace Store.Service.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductAsync(ProductSpecification productSpecs);
        Task<IReadOnlyList<CategoryDetailsDto>> GetAllCategoriesAsync();
       
    }
}
