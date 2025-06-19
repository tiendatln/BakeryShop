using AutoMapper;
using OrderAPI.DTOs;
using OrderAPI.Models;

namespace OrderAPI.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<UpdateOrderDTO, Order>();
            CreateMap<Order, ReadOrderDTO>();

            CreateMap<CreateOrderDetailDTO, OrderDetail>();
            CreateMap<UpdateOrderDetailDTO, OrderDetail>();
            CreateMap<OrderDetail, ReadOrderDetailDTO>();
        }
    }
}
