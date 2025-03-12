using AutoMapper;
using OrderService.Application.DTOs.Address;
using OrderService.Domain.Entities.OrderComponents;

namespace OrderService.Application.MappingProfiles
{
    public class AddressProfile: Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressRequestDto>().ReverseMap();
        }
    }
}
