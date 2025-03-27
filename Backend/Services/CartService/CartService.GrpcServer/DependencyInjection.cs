using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using CartService.Infrastructure.Implementations.Repositories;
using CartService.Infrastructure.Implementations;
using StackExchange.Redis;
using CartService.Application.Specifications.Jobs;
using CartService.Infrastructure.Implementations.Jobs;
using CartService.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CartService.GrpcServer
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IJobRepository, CartJobRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var connectionString = configuration.GetConnectionString("Hangfire");
            services.AddDbContext<HangfireDbContext>(options => options.UseSqlServer(connectionString));
            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();

            services.AddScoped<ICartJobService, CartJobService>();
        }
    }
}
