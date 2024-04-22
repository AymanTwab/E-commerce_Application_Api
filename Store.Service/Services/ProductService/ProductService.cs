using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.ProductService;
using Store.Service.Helper;
using Store.Service.Services.ProductService.Dtos;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<CategoryDetailsDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
            var mappedCategories = _mapper.Map<IReadOnlyList<CategoryDetailsDto>>(categories);
            return mappedCategories;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductAsync(ProductSpecification productSpecs)
        {
            var specs = new ProductsWithSpecifications(productSpecs);
            var products = await _unitOfWork.Repository<Product, int>().GetAllSpecificationsAsync(specs);
            var countSpecs = new ProductsWithFilterAndCountSpecifications(productSpecs);
            var count = await _unitOfWork.Repository<Product,int>().CountWithSpecificationAsync(countSpecs);
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
            return new PaginatedResultDto<ProductDetailsDto>(productSpecs.PageIndex,productSpecs.PageSize, count, mappedProducts);
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if (id == null)
                throw new Exception("Error");

            var specs = new ProductsWithSpecifications(id);
            var product = await _unitOfWork.Repository<Product, int>().GetByIdWithSpecificationsAsync(specs);
            var mappedProduct = _mapper.Map<ProductDetailsDto>(product);
            return mappedProduct;
        }

    }
}
