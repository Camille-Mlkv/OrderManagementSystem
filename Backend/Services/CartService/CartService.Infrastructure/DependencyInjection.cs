using CartService.Application.Specifications.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using CartService.Infrastructure.Implementations.Repositories;
using Hangfire;
using CartService.Application.Specifications;
using CartService.Infrastructure.Implementations;
using CartService.Application.Specifications.Jobs;
using CartService.Infrastructure.Implementations.Jobs;

namespace CartService.Infrastructure
{
    public static class DependencyInjection
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

        public static void ConfigureHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Hangfire");

            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
        }
    }
}
