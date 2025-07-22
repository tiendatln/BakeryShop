﻿using AutoMapper;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDTO, Product>();

            CreateMap<UpdateProductDTO, Product>();

            CreateMap<Product, ReadProductDTO>();

            CreateMap<Product, UpdateProductDTO>()
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore()); // ✅ fix lỗi

            CreateMap<ReadProductDTO, Product>();
        }
    }
}
