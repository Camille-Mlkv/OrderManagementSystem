namespace OrderService.Domain.Entities.OrderComponents
{
    public class OrderMeal
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PortionsAmount { get; set; }
    }
}
