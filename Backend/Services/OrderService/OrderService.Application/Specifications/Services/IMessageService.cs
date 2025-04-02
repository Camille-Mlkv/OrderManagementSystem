using OrderService.Application.DTOs.Order;

namespace OrderService.Application.Specifications.Services
{
    public interface IMessageService
    {
        Task PublishAsync(OrderStatusDto orderStatusDto);
    }
}
