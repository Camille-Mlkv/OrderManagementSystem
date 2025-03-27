using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using CartService.Infrastructure.Implementations.Jobs;
using CartService.Infrastructure.Implementations.Repositories;
using CartService.Infrastructure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CartService.Infrastructure.DI
{
    public static class CartServicesConfiguration
    {
        public static void ConfigureCartServices(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IJobRepository, CartJobRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICartJobService, CartJobService>();
        }
    }
}
