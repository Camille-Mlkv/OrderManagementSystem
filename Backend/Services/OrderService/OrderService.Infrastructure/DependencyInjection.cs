using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderService.Application.Specifications.Repositories;
using OrderService.Infrastructure.Implementations.Repositories;
using Microsoft.Extensions.Configuration;
using OrderService.Application.Specifications.Services;
using OrderService.Infrastructure.Implementations.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void ConfigureDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb");

            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase("OrderDatabase");

            services.AddSingleton<IMongoDatabase>(database);

            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void ConfigureStripeSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped<IPaymentService, StripePaymentService>();
        }

        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection("JwtOptions");

            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorizationBuilder()
               .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
               .AddPolicy("Courier", policy => policy.RequireRole("Courier"))
               .AddPolicy("Client", policy => policy.RequireRole("Client"));
        }
    }
}
