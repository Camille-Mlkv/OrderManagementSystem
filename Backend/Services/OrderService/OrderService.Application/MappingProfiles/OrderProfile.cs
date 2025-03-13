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
        }
    }
}
