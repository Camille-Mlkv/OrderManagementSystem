using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure.DI
{
    public static class HangfireConfiguration
    {
        public static void ConfigureHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Hangfire");

            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
        }
    }
}
