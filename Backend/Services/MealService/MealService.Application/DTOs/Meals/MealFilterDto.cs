namespace MealService.Application.DTOs.Meals
{
    public class MealFilterDto
    {
        public Guid CuisineId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsAvailable { get; set; } = false;
    }
}
