namespace OrderService.Application.DTOs.Meal
{
    public class OrderMealDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PortionsAmount { get; set; }
    }
}
