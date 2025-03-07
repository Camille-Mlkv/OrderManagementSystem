namespace CartService.Domain.Entities
{
    public class Cart
    {
        public string UserId { get; set; } = default!;
        public List<CartItem> Items { get; private set; } = new();

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

        public void RemoveItem(Guid itemId)
        {
            Items.RemoveAll(x => x.MealId == itemId);
        }

        public void Clear()
        {
            Items.Clear();
        }

    }
}
