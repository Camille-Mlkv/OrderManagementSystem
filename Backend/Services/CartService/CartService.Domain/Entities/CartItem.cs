namespace CartService.Domain.Entities
{
    public class CartItem
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
    }

}
