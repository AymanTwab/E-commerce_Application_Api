using Store.Data.Entities;

namespace Store.Repository.Specification.ProductService
{
    public class ProductsWithFilterAndCountSpecifications : BaseSpecification<Product>
    {
        public ProductsWithFilterAndCountSpecifications(ProductSpecification specs)
            : base(product => !specs.CategoryId.HasValue || specs.CategoryId == product.CategoryId)
        {
        }
    }
}
