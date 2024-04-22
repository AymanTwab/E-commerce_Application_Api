using Store.Data.Entities;

namespace Store.Repository.Specification.ProductService
{
    public class ProductsWithSpecifications : BaseSpecification<Product>
    {
        public ProductsWithSpecifications(ProductSpecification specs)
            : base(product => !specs.CategoryId.HasValue || specs.CategoryId == product.CategoryId)
        {
            AddInclude(x => x.Category);
            AddOrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending (x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.Name);
            }

            ApplyPagination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);
        }

        public ProductsWithSpecifications(int? id)
            : base(product => product.Id == id)
        {
            AddInclude(x => x.Category);
        }
    }
}
