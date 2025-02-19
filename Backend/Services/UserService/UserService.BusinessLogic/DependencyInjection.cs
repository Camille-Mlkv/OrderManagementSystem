using Microsoft.Extensions.DependencyInjection;
using UserService.BusinessLogic.Implementations.Repositories;
using UserService.BusinessLogic.Specifications.Repositories;
using UserService.BusinessLogic.Specifications.Services;
using UserService.BusinessLogic.Implementations.Services;
using UserService.BusinessLogic.MappingProfiles;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace UserService.BusinessLogic
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(UserProfile));
        }

        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
