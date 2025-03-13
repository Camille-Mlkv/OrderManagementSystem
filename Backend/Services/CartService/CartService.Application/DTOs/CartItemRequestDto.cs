namespace CartService.Application.DTOs
{
    public class CartItemRequestDto
    {
        public Guid MealId { get; set; }
        public int Quantity { get; set; }
    }
}
