using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CartService.Application
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void ConfigureGrpcConnection(this IServiceCollection services, IConfiguration configuration)
        {
             services.AddGrpcClient<MealService.GrpcServer.MealService.MealServiceClient>(options =>
             {
                 options.Address = new Uri(configuration["MealGrpcUrl"]!);

             }).ConfigurePrimaryHttpMessageHandler(() =>
             {
                 var handler = new HttpClientHandler();

                 handler.ServerCertificateCustomValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => true;

                 return handler;
             });
        }
    }
}
