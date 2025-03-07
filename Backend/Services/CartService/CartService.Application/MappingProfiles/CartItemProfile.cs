using AutoMapper;
using CartService.Application.DTOs;
using CartService.Domain.Entities;

namespace CartService.Application.MappingProfiles
{
    public class CartItemProfile: Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem,CartItemDto>().ReverseMap();
        }
    }
}
