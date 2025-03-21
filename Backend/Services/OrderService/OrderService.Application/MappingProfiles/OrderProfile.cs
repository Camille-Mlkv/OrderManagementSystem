using AutoMapper;
using OrderService.Application.DTOs.Order;
using OrderService.Domain.Entities;

namespace OrderService.Application.MappingProfiles
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OpenedOrderDto>().ReverseMap();

            CreateMap<Order, OrderDto>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name.ToString()));
        }
    }
}
