using CartService.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure.DI
{
    public static class HangfireConfiguration
    {
        public static void ConfigureHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Hangfire");

            services.AddDbContext<HangfireDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Hangfire")));

            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));

            services.AddHangfireServer();
        }
    }
}
