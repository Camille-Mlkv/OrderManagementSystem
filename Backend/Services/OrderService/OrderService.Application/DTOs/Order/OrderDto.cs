using OrderService.Application.DTOs.Address;
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
        public AddressDto Address { get; set; } = new();
        public List<OrderMealDto> Meals { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
