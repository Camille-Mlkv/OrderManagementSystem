using Microsoft.Extensions.DependencyInjection;
using UserService.DataAccess.Implementations.Repositories;
using UserService.DataAccess.Specifications.Repositories;
using UserService.DataAccess.Specifications;
using UserService.DataAccess.Implementations;

namespace UserService.DataAccess.DI
{
    public static class PersistenceConfiguration
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
