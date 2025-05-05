using OrderService.Application.DTOs.Order;
using OrderService.Domain.Entities;

namespace OrderService.Application.Specifications.Services
{
    public interface IOrderNotificationService
    {
        Task NotifyOrderUpdatedAsync(OrderDto order);
    }
}
