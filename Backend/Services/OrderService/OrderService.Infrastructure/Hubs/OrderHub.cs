using Microsoft.AspNetCore.SignalR;

namespace OrderService.Infrastructure.Hubs
{
    public class OrderHub : Hub<IOrderClient>
    {
    }
}
