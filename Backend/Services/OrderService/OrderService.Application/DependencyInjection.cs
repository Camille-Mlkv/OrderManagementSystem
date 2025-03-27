using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Utilities;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<OrderNumberGenerator>();
        }

        public static void ConfigureCartGrpcService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<CartService.GrpcServer.CartService.CartServiceClient>(options =>
            {
                options.Address = new Uri(configuration["CartGrpcUrl"]!);

            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();

                handler.ServerCertificateCustomValidationCallback =
                   (sender, certificate, chain, sslPolicyErrors) => true;

                return handler;
            });
        }

        public static void ConfigureMealGrpcService(this IServiceCollection services, IConfiguration configuration)
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

        public static void ConfigureUserGrpcService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<UserService.GrpcServer.UserService.UserServiceClient>(options =>
            {
                options.Address = new Uri(configuration["UserGrpcUrl"]!);

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
