using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Domain.Entities;

namespace MealService.Application.MappingProfiles
{
    public class CuisineProfile:Profile
    {
        public CuisineProfile()
        {
            CreateMap<Cuisine,CuisineDto>().ReverseMap();
            CreateMap<Cuisine,CuisineRequestDto>().ReverseMap();    
        }
    }
}
