using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Specifications.Services;
using OrderService.Infrastructure.Implementations.Services.SignalR;

namespace OrderService.Infrastructure.DI
{
    public static class SignalRConfiguration
    {
        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<IOrderNotificationService, OrderNotificationService>();
        }
    }
}
