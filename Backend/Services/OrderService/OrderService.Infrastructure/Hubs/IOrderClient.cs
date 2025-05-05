using OrderService.Application.DTOs.Order;

namespace OrderService.Infrastructure.Hubs
{
    public interface IOrderClient
    {
        Task OrderUpdated(OrderDto order);
    }
}
