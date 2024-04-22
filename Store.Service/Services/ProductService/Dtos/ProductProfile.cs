using AutoMapper;
using Store.Data.Entities;

namespace Store.Service.Services.ProductService.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDetailsDto>()
                 .ForMember(dest => dest.CategoryName,options => options.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.ImageUrl,options => options.MapFrom<ProductUrlResolver>());

            CreateMap<Category, CategoryDetailsDto>();
        }
    }
}
