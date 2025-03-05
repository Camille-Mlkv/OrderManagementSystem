using AutoMapper;
using MealService.Application.DTOs.Tags;
using MealService.Domain.Entities;

namespace MealService.Application.MappingProfiles
{
    public class TagProfile:Profile
    {
        public TagProfile()
        {
            CreateMap<Tag,TagDto>().ReverseMap();
            CreateMap<Tag, TagRequestDto>().ReverseMap();
        }
    }
}
