using Microsoft.AspNetCore.SignalR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Services;
using OrderService.Infrastructure.Hubs;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Implementations.Services.SignalR
{
    public class OrderNotificationService: IOrderNotificationService
    {
        private readonly IHubContext<OrderHub, IOrderClient> _hubContext;

        public OrderNotificationService(IHubContext<OrderHub, IOrderClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyOrderUpdatedAsync(OrderDto order)
        {
            await _hubContext.Clients.All.OrderUpdated(order);
        }
    }
}
