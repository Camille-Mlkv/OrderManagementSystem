using MealService.Application.Specifications;
using MealService.Application.Specifications.Repositories;
using MealService.Application.Specifications.Services;
using MealService.Infrastructure.Implementations;
using MealService.Infrastructure.Implementations.Repositories;
using MealService.Infrastructure.Implementations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MealService.Infrastructure.DI
{
    public static class PersistenceConfiguration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddSingleton<IImageService, CloudinaryService>();
        }
    }
}
