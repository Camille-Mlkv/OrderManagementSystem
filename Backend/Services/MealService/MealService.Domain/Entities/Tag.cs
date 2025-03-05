namespace MealService.Domain.Entities
{
    public class Tag: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MealTag> MealTags { get; set; } = new List<MealTag>();
    }
}
