namespace MealService.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
