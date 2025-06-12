using AutoMapper;
using CartAPI.DTOs;
using CartAPI.Models;

namespace CartAPI.Profiles
{
    public class CartPoriles : Profile
    {
        public CartPoriles()
        {
            // Map từ Cart (entity) sang CartDTO (trả về client)
            CreateMap<Cart, CartDTO>();

            // Map từ CartCreateDTO (input) sang Cart (entity)
            CreateMap<CartCreateDTO, Cart>();

            // Map từ CartQuantityUpdateDTO sang Cart nếu cần
            // (Thông thường dùng thủ công hoặc map từng field)
            CreateMap<CartQuantityUpdateDTO, Cart>()
                .ForMember(dest => dest.CartID, opt => opt.MapFrom(src => src.CartID))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
