using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DataAccess.Data;

namespace UserService.DataAccess.DI
{
    public static class DbConfiguration
    {
        public static IServiceCollection AddDbProvider(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
            });
            return services;
        }
    }
}
