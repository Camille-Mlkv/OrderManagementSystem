using FluentValidation.AspNetCore;
using FluentValidation;
using MealService.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MealService.Application
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(typeof(MealProfile), typeof(CategoryProfile));

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
