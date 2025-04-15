using OrderService.Application.DTOs.Meal;

namespace OrderService.Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public Guid? CourierId { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public List<OrderMealDto> Meals { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
