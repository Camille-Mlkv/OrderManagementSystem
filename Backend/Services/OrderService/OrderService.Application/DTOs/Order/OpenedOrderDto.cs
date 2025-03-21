using OrderService.Application.DTOs.Address;

namespace OrderService.Application.DTOs.Order
{
    public class OpenedOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public AddressDto Address { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
