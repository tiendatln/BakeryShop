using AutoMapper;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
            CreateMap<Category, ReadCategoryDTO>()
                .ForMember(dest => dest.ProductCount,
                          opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));
        }
    }
}