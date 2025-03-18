using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderService.Application.Specifications.Repositories;
using OrderService.Infrastructure.Implementations.Repositories;
using Microsoft.Extensions.Configuration;
using OrderService.Application.Specifications.Services;
using OrderService.Infrastructure.Implementations.Services;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void ConfigureDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb");

            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase("OrderDatabase");

            services.AddSingleton<IMongoDatabase>(database);

            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void ConfigureStripeSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings")); // solve secrets issue

            services.AddScoped<IPaymentService, StripePaymentService>();
        }
    }
}
