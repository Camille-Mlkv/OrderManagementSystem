namespace MealService.Application.DTOs.Meals
{
    public class MealFilterDto
    {
        public Guid? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public List<Guid> TagIds { get; set; } = new List<Guid>();
        public double? MaxPrice { get; set; }
        public double? MinPrice { get; set; }
        public int? MaxCalories { get; set; }
        public int? MinCalories { get; set; }
    }
}
