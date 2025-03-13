using CartService.Application.Specifications.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using CartService.Infrastructure.Implementations.Repositories;
using Hangfire;
using CartService.Application.Specifications;
using CartService.Infrastructure.Implementations;
using CartService.Application.Specifications.Jobs;
using CartService.Infrastructure.Implementations.Jobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CartService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void ConfigureCartServices(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IJobRepository, CartJobRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICartJobService, CartJobService>();
        }

        public static void ConfigureHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Hangfire");

            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
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
