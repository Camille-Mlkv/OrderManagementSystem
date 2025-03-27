using AutoMapper;
using OrderService.Application.DTOs.Meal;
using OrderService.Domain.Entities.OrderComponents;

namespace OrderService.Application.MappingProfiles
{
    public class MealProfile: Profile
    {
        public MealProfile()
        {
            CreateMap<OrderMeal, OrderMealDto>().ReverseMap();
        }
    }
}
