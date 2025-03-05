namespace MealService.Domain.Entities
{
    public class Meal:BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public Guid CuisineId { get; set; }
        public virtual Cuisine Cuisine { get; set; }

        public ICollection<MealTag> MealTags { get; set; } = new List<MealTag>();

        public void SetTags(IEnumerable<Tag> tags)
        {
            MealTags.Clear();
            foreach (var tag in tags)
            {
                MealTags.Add(new MealTag
                {
                    Id = Guid.NewGuid(),
                    MealId = Id,
                    TagId = tag.Id
                });
            }
        }
    }
}
