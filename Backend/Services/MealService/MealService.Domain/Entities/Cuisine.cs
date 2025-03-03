namespace MealService.Domain.Entities
{
    public class Cuisine : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }

}
