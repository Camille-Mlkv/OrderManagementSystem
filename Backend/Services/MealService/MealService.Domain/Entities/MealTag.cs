namespace MealService.Domain.Entities
{
    public class MealTag: BaseEntity
    {
        public Guid MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
