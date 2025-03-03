using Microsoft.AspNetCore.Http;

namespace MealService.Application.DTOs.Meals
{
    public class MealRequestDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Guid CategoryId { get; set; }
    }
}
