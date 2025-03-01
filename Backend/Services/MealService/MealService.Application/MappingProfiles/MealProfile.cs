using AutoMapper;
using MealService.Application.DTOs;
using MealService.Domain.Entities;

namespace MealService.Application.MappingProfiles
{
    public class MealProfile:Profile
    {
        public MealProfile()
        {
            CreateMap<Meal,MealDto>().ReverseMap();
        }
    }
}
