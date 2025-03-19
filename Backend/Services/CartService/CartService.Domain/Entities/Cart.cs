namespace CartService.Domain.Entities
{
    public class Cart
    {
        public Guid? UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(x => x.MealId == item.MealId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(Guid mealId)
        {
            Items.RemoveAll(i => i.MealId == mealId);
        }

        public CartItem? GetItemById(Guid mealId)
        {
            return Items.FirstOrDefault(i => i.MealId == mealId);
        }
    }
}
