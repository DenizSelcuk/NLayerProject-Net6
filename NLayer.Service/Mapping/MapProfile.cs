using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;

namespace NLayer.Service.Mapping 
{
    //Automapper profile sınıfından miras alarak bir mapprofile sınıfı oluşturduk. Burada hangi modelleri hangi modellere göre mapleyebileceğimizi yazdık. Programc.cs'de automapper eklenerek bu sınıf verilmeli.
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto,Product>();
            CreateMap<Product, ProductWithCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryWithProductsDto>();
        }
    }
}
