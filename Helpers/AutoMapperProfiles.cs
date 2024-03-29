using API.DTOs;
using API.DTOs.Product;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<RegisterDto, AppUser>();

            CreateMap<Photo, PhotoDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductCreateDto, Product>();

            CreateMap<AppUser, ProfileDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Product, ProductDto>()
            //  .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
             .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)
             .Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList()));

        }
    }
}