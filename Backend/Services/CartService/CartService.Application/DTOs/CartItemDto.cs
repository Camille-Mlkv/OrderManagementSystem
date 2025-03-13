namespace CartService.Application.DTOs
{
    public class CartItemDto
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
    }
}
