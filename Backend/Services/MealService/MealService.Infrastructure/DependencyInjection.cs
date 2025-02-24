using MealService.Application.Specifications;
using MealService.Application.Specifications.Repositories;
using MealService.Infrastructure.Data;
using MealService.Infrastructure.Implementations;
using MealService.Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MealService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void ConfigureDbConnection(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
