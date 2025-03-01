using AutoMapper;
using MealService.Application.DTOs;
using MealService.Domain.Entities;

namespace MealService.Application.MappingProfiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();
        }
    }
}
