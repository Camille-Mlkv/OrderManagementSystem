using Microsoft.Extensions.DependencyInjection;
using UserService.DataAccess.Implementations.Repositories;
using UserService.DataAccess.Specifications.Repositories;
using UserService.DataAccess.Specifications;
using UserService.DataAccess.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using UserService.DataAccess.Data;

namespace UserService.DataAccess.DI
{
    public static class PersistenceConfiguration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
