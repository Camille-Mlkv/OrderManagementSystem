using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DataAccess.Implementations.Services.RabbitMQ;
using UserService.DataAccess.Options;
using UserService.DataAccess.Specifications.Services;

namespace UserService.DataAccess.DI
{
    public static class RabbitMQConfiguration
    {
        public static void ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQOptions>(configuration.GetSection("MessageBroker"));

            services.AddSingleton<IRabbitMQService, RabbitMQService>();
        }
    }
}
