using MealService.Application.DTOs.Tags;

namespace MealService.Application.DTOs.Meals
{
    public class MealDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CuisineId { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
